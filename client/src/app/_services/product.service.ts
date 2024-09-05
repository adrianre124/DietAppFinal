import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Product } from '../_models/product';
import { Observable, catchError, of } from 'rxjs';
import { SearchProduct } from '../_models/searchProduct';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getProduct(productId: number) {
    return this.http.get<Product>(this.apiUrl + 'product/' + productId);
  }

  getProductNutrition(productName: string, weight: number): Observable<any> {
    return this.http.get<Product>(`${this.apiUrl}product/${productName}/${weight}`);
  }

  getUserProducts() {
    return this.http.get<Product[]>(this.apiUrl + 'product');
  }

  searchProducts(query: string): Observable<any> {
    return this.http.get<Product[]>(this.apiUrl + 'product/search?productName=' + query).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          console.log('Product not found');
          return of([]);
        }
        console.error('An error occurred: ', error.message);
        return of([]);
      })
    );
  }

  addProduct(formGroup: any) {
    return this.http.post(`${this.apiUrl}product/add-product`, formGroup);
  }

  updateProduct(productId: number, product: any) {
    return this.http.put(this.apiUrl + 'product/' + productId, product);
  }

  deleteProduct(productId: number) {
    return this.http.delete(this.apiUrl + 'product/' + productId);
  }
}
