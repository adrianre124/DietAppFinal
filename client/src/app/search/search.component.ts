import { Component, output } from '@angular/core';
import { Product } from '../_models/product';
import { SearchProduct } from '../_models/searchProduct';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import { ProductService } from '../_services/product.service';
import { DecimalPipe, NgFor, NgIf, SlicePipe } from '@angular/common';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, NgIf, NgFor, SlicePipe, DecimalPipe],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  productName = new FormControl('');
  weight = new FormControl(100);
  nutritionFacts: any;
  noProductsFound: boolean = false;
  products: Product[] = [];
  selectedProduct: Product | null = null;
  product = output<Product>();

  constructor(private productService: ProductService) {
    this.setupSerach();
  }

  setupSerach() {
    this.productName.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => {
        if (!query) {
          this.noProductsFound = true;
          return of([]);
        }

        return this.productService.searchProducts(query);
      })
    ).subscribe(response => {
      console.log('Updating serachResults$', response);
      this.products = response;

      if (this.products.length === 0) {
        this.noProductsFound = true;
      } else {
        this.noProductsFound = false;
      }
    },
    )
  }

  toggleDropdown(results: any[]) {
    this.noProductsFound = results.length > 0;
  }

  addProduct() {
    if (this.selectedProduct) {
      let product: Product = {...this.selectedProduct, weight: this.weight.value!};
      this.product.emit(product);
    }
  }

  selectProduct(prod: Product) {
    if (prod) {
      if (!prod.productName) {
        prod.productName = '(No name)';
      }
      this.productName.setValue(prod.productName, { emitEvent: false });
      this.selectedProduct = prod;
      this.noProductsFound = false;
    } else {
      console.error('Invalid product selected');
    }
  }

  onFocus() {
    of(this.products).subscribe(results => {
      this.toggleDropdown(results);
    })
  }

  closeDropdown() {
    this.noProductsFound = false;
    this.products = [];
  }
}
