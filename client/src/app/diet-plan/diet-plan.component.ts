import { Component, OnInit, QueryList, ViewChildren, inject } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DietPlan } from '../_models/dietPlan';
import { DietPlanService } from '../_services/diet-plan.service';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { MealComponent } from '../meal/meal.component';
import { Product } from '../_models/product';
import { Meal } from '../_models/meal';
import { ProductListComponent } from '../product-list/product-list.component';
import { DietPlanListComponent } from '../diet-plan-list/diet-plan-list.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash, faEdit, faSave, faPlus, faWindowClose } from '@fortawesome/free-solid-svg-icons';
import { ProductSearchComponent } from "../product-search/product-search.component";
import { SearchComponent } from '../search/search.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-diet-plan',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, TextInputComponent, NgIf, NgFor, MealComponent, DecimalPipe, ProductListComponent,
    DietPlanListComponent, FontAwesomeModule, ProductSearchComponent, SearchComponent],
  templateUrl: './diet-plan.component.html',
  styleUrl: './diet-plan.component.css'
})
export class DietPlanComponent implements OnInit{
  faTrash = faTrash; faEdit = faEdit; faSave = faSave; faPlus = faPlus; faWindowClose = faWindowClose;
  private toastr = inject(ToastrService);
  private dietPlanService = inject(DietPlanService);
  @ViewChildren(ProductListComponent) productListComponents: QueryList<ProductListComponent> | undefined;
  dietPlanForm: FormGroup = new FormGroup({});
  dietPlans: DietPlan[] = [];
  currentDietPlan: DietPlan | null = null;
  showChangeName = false;
  editName: string = '';
  totalNutrition: any = {
    calories: 0,
    proteins: 0,
    carbohydrates: 0,
    fats: 0,
    sugars: 0,
    salt: 0
  };

  ngOnInit(): void {
    this.loadDietPlans();
  }

  loadDietPlans() {
    this.dietPlanService.getDietPlans().subscribe({
      next: response => {
        this.dietPlans = response;
        if (this.dietPlans.length > 0) {
          this.selectNewestDietPlan();
        }
      },
      error: error => {
        console.error('Error loading diet plants: ', error);
      }
    })
  }

  selectNewestDietPlan() {
    this.currentDietPlan = this.dietPlans.reduce((latest, current) => {
      return (current.createDate > latest.createDate) ? current : latest;
    });
    this.calculateSummedNutritionalValues();
  }

  onDietPlanSelected(dietPlan: DietPlan) {
    this.currentDietPlan = dietPlan;
    this.calculateSummedNutritionalValues();
    console.log(dietPlan);
  }

  onDietPlanCreated(dietPlan: DietPlan) {
    this.dietPlans.unshift(dietPlan);
    this.currentDietPlan = dietPlan;
    this.calculateSummedNutritionalValues();
  }

  calculateSummedNutritionalValues() {
    this.totalNutrition = {
      calories: 0,
      proteins: 0,
      carbohydrates: 0,
      fats: 0,
      sugars: 0,
      salt: 0
    };
    this.currentDietPlan?.meals.forEach((meal: Meal) => {
      meal.products.forEach((product: Product) => {
        this.updateSummedNutritionValues(product, true);
      });
    });
  }

  changeDietPlanName(): void {
    const name = this.dietPlanForm.value.name;
    this.dietPlanService.createDietPlan(name).subscribe({
      next: dietPlan => {
        this.dietPlans.push(dietPlan);
        this.currentDietPlan = dietPlan;
      }
    })
  }

  saveChanges() {
    console.log(this.currentDietPlan);
    if (this.currentDietPlan != null) {
      this.dietPlanService.updateDietPlan(this.currentDietPlan).subscribe({
        next: () => this.toastr.success("Diet Plan successfully updated", 'Success'),
        error: () => this.toastr.error("Error Saving Diet Plan Changes", 'Error')
      });
    }
  }

  deleteMeal(id: number) {
    var meal = this.currentDietPlan?.meals.splice(id, 1)[0];
    meal?.products.forEach((product: Product) => {
      this.updateSummedNutritionValues(product, false);
    });
    console.log(this.currentDietPlan);
  }

  toggleChangeName() {
    this.showChangeName = !this.showChangeName;
    this.editName = this.currentDietPlan?.name || '';
  }

  saveName() {
    if (this.currentDietPlan) {
      this.currentDietPlan.name = this.editName;
      this.toggleChangeName();
    }
  }

  deleteDietPlan(id: number) {
    const confirmed = window.confirm('Are you sure you want to delete this Diet Plan?');

    if (confirmed) {
      this.dietPlanService.removeDietPlan(id).subscribe({
        next: () => {
          this.dietPlans = this.dietPlans.filter(dp => dp.dietPlanId !== id);
          if (this.dietPlans.length > 0) {
            this.currentDietPlan = this.dietPlans[0];
          } else {
            this.currentDietPlan = null;
          }
        }
      });
    }
  }

  onMealAdded(meal: any) {
    const newMeal: any  = {
      name: meal.name,
      products: []
    };
    console.log(newMeal);
    this.currentDietPlan?.meals.push(newMeal);
    console.log(this.currentDietPlan?.meals);
  }

  onProductAdd(product: Product, index: number) {
    if (this.productListComponents) {
      const component = this.productListComponents.toArray()[index];
      if (component) {
        component.addProduct(product);
        this.updateSummedNutritionValues(product, true);
      }
    }
  }

  onProductDeleted(product: Product, index: number) {
    this.updateSummedNutritionValues(product, false);
  }

  trackByMealId(index: number, meal: Meal): number {
    return meal.mealId;
  }

  private updateSummedNutritionValues(data: Product, add: boolean) {
    if (add) {
      this.totalNutrition.calories += this.safeValue(data.calories)
      this.totalNutrition.proteins += this.safeValue(data.proteins);
      this.totalNutrition.carbohydrates += this.safeValue(data.carbohydrates);
      this.totalNutrition.fats += this.safeValue(data.fats);
      this.totalNutrition.sugars += this.safeValue(data.sugars);
      this.totalNutrition.salt += this.safeValue(data.salt);
    } else {
      this.totalNutrition.calories -= this.safeValue(data.calories);
      this.totalNutrition.proteins -= this.safeValue(data.proteins);
      this.totalNutrition.carbohydrates -= this.safeValue(data.carbohydrates);
      this.totalNutrition.fats -= this.safeValue(data.fats);
      this.totalNutrition.sugars -= this.safeValue(data.sugars);
      this.totalNutrition.salt -= this.safeValue(data.salt);
    }
  }

  private safeValue(value: number | undefined | null): number {
    return isNaN(value as number) || value === null || value === undefined ? 0 : value as number;
  }
}
