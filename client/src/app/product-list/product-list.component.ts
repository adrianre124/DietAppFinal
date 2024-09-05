import { Component, OnInit, input, output } from '@angular/core';
import { Product } from '../_models/product';
import { DecimalPipe, NgFor, NgIf, UpperCasePipe } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { SearchComponent } from '../search/search.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [NgFor, NgIf, FontAwesomeModule, DecimalPipe, UpperCasePipe, SearchComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit{
  faTrash = faTrash;
  products = input<Product[]>([]);
  product = input<Product | null>(null);
  deletedProduct = output<Product>();
  summedNutritionalValues: any = {
    calories: 0,
    proteins: 0,
    carbohydrates: 0,
    fats: 0,
    sugars: 0,
    salt: 0
  };

  ngOnInit(): void {
    this.initializeNutritionalValues();
  }

  initializeNutritionalValues() {
      this.products().forEach((product: Product) => {
        this.updateSummedNutritionValues(product, true);
      });
  }

  addProduct(product: Product) {
    product = this.calculateProductPerWeight(product);
    this.products().push(product);
    this.updateSummedNutritionValues(product, true);
  }

  deleteProduct(productId: number) {
    console.log(`Deleting product: ${productId}`);
    console.log(this.products);
    var product = this.products().splice(productId, 1);
    this.updateSummedNutritionValues(product[0], false);
    this.deletedProduct.emit(product[0]);
}


isNumber(value: any): boolean {
  return !isNaN(parseFloat(value)) && isFinite(value);
}

private updateSummedNutritionValues(data: Product, add: boolean) {
  if (add) {
    this.summedNutritionalValues.calories += this.safeValue(data.calories)
    this.summedNutritionalValues.proteins += this.safeValue(data.proteins);
    this.summedNutritionalValues.carbohydrates += this.safeValue(data.carbohydrates);
    this.summedNutritionalValues.fats += this.safeValue(data.fats);
    this.summedNutritionalValues.sugars += this.safeValue(data.sugars);
    this.summedNutritionalValues.salt += this.safeValue(data.salt);
  } else {
    this.summedNutritionalValues.calories -= this.safeValue(data.calories);
    this.summedNutritionalValues.proteins -= this.safeValue(data.proteins);
    this.summedNutritionalValues.carbohydrates -= this.safeValue(data.carbohydrates);
    this.summedNutritionalValues.fats -= this.safeValue(data.fats);
    this.summedNutritionalValues.sugars -= this.safeValue(data.sugars);
    this.summedNutritionalValues.salt -= this.safeValue(data.salt);
  }
}

private calculateProductPerWeight(product: Product) {
  product.calories = this.safeValue(product.calories) * (product.weight / 100)
  product.proteins = this.safeValue(product.proteins) * (product.weight / 100);
  product.carbohydrates = this.safeValue(product.carbohydrates) * (product.weight / 100);
  product.fats = this.safeValue(product.fats) * (product.weight / 100);
  product.sugars = this.safeValue(product.sugars) * (product.weight / 100);
  product.salt = this.safeValue(product.salt) * (product.weight / 100);

  return product;
}

private safeValue(value: number | undefined | null): number {
  return isNaN(value as number) || value === null || value === undefined ? 0 : value as number;
}
}
