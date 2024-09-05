import { Component, ViewChild, input } from '@angular/core';
import { Product } from '../_models/product';
import { AsyncPipe, DecimalPipe, NgFor, NgIf, SlicePipe } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ProductListComponent } from '../product-list/product-list.component';
import { SearchComponent } from "../search/search.component";

@Component({
  selector: 'app-product-search',
  standalone: true,
  imports: [NgIf, NgFor, ReactiveFormsModule, TextInputComponent, AsyncPipe, SlicePipe, DecimalPipe, FontAwesomeModule, ProductListComponent, SearchComponent],
  templateUrl: './product-search.component.html',
  styleUrl: './product-search.component.css'
})
export class ProductSearchComponent {
  faTrash = faTrash;
  @ViewChild(ProductListComponent) productListComponent: ProductListComponent | undefined;
  nutritionFacts: any;
  noProductsFound: boolean = false;
  products: Product[] = [];
  product: Product | null = null;

  summedNutritionalValues = {
    calories: 0,
    proteins: 0,
    carbohydrates: 0,
    fats: 0,
    sugars: 0,
    salt: 0
  }

  onProductAdd($event: Product) {
    // this.products.push($event);
    // this.product = $event;

    if (this.productListComponent) {
      this.productListComponent.addProduct($event);
    }
  }
}
