import { Component, OnInit, inject } from '@angular/core';
import { ProductService } from '../_services/product.service';
import { ToastrService } from 'ngx-toastr';
import { Product } from '../_models/product';
import { DecimalPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { EditProductComponent } from "../edit-product/edit-product.component";

@Component({
  selector: 'app-user-products',
  standalone: true,
  imports: [NgIf, NgFor, FormsModule, DecimalPipe, FontAwesomeModule, EditProductComponent],
  templateUrl: './user-products.component.html',
  styleUrl: './user-products.component.css'
})
export class UserProductsComponent implements OnInit{
  faPlus = faPlus;
  private productService = inject(ProductService);
  private toastr = inject(ToastrService);
  private router = inject(Router);
  products: Product[] = [];
  currentProduct: Product | null = null;
  editToggle = false;

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getUserProducts().subscribe({
      next: (data: Product[]) => this.products = data,
      error: () => this.toastr.error('Failed to load products', 'Error')
    });
  }

  addProduct() {
    this.router.navigateByUrl('/product/create');
  }

  editProduct(index: number): void {
    this.currentProduct = this.products[index];
    this.toggleEdit();
  }

  toggleEdit() {
    this.editToggle = !this.editToggle;
  }

  deleteProduct(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          this.toastr.success('Product deleted successfully', 'Success');
          this.products = this.products.filter(p => p.productId != id);
        },
        error: () => {
          this.toastr.error('Failed to delete product', 'Error');
        }
      });
    }
  }

}
