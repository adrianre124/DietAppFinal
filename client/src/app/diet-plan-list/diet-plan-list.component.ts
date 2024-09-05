import { Component, OnInit, inject, input, output } from '@angular/core';
import { DietPlan } from '../_models/dietPlan';
import { DietPlanService } from '../_services/diet-plan.service';
import { NgFor, NgIf } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TextInputComponent } from "../_forms/text-input/text-input.component";

@Component({
  selector: 'app-diet-plan-list',
  standalone: true,
  imports: [NgIf, NgFor, RouterLink, FontAwesomeModule, ReactiveFormsModule, TextInputComponent],
  templateUrl: './diet-plan-list.component.html',
  styleUrl: './diet-plan-list.component.css'
})
export class DietPlanListComponent implements OnInit{
  faPlus = faPlus;
  private dietPlanService = inject(DietPlanService);
  private fb = inject(FormBuilder);
  // private route = inject(ActivatedRoute);
  // private router = inject(Router);
  dietPlans = input<DietPlan[]>([]);
  currentDietPlan: DietPlan | null = null;
  selectedDietPlan = output<DietPlan>();
  createdDietPlan = output<DietPlan>();
  dietPlanForm: FormGroup = new FormGroup({});
  loadFormBool = false;

  ngOnInit(): void {
    this.initializeDietPlanForm();
  }

  initializeDietPlanForm() {
    this.dietPlanForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  onDietPlanClick(dietPlan: DietPlan) {
    this.currentDietPlan = dietPlan;
    this.selectedDietPlan.emit(dietPlan);
  }

  addDietPlan() {
    console.log(this.dietPlanForm.value);
    if (this.dietPlanForm.valid) {
      this.dietPlanService.createDietPlan(this.dietPlanForm.value).subscribe({
        next: (response: any) => {
          console.log('DietPlan added successfully:', response);
          this.createdDietPlan.emit(response);
          this.dietPlanForm.reset();
        },
        error: (error: any) => {
          console.error('Error adding DietPlan: ', error);
        }
      });
    } else {
      console.error('Form is invalid');
    }

    this.loadFormBool = !this.loadFormBool;
  }

  loadForm() {
    this.loadFormBool = !this.loadFormBool;
  }
}
