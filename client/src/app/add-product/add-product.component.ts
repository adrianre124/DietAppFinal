import { Component, OnInit, inject, output } from '@angular/core';
import { ProductService } from '../_services/product.service';
import { Product } from '../_models/product';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { JsonPipe, NgIf } from '@angular/common';
import { TextInputComponent } from '../_forms/text-input/text-input.component';
import { FileUploadModule, FileUploader } from 'ng2-file-upload';
import { environment } from '../../environments/environment.development';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [ReactiveFormsModule, JsonPipe, NgIf, TextInputComponent, PhotoEditorComponent],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent implements OnInit{
  private productService = inject(ProductService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  cancelAddProduct = output<boolean>();
  addProductForm: FormGroup = new FormGroup([]);
  validationErrors: string[] | undefined;
  selectedFile: File | null = null;
  isLoading = false;

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.addProductForm = this.fb.group({
      productName: ['', Validators.required],
      calories: [0],
      proteins: [0],
      carbohydrates: [0],
      fats: [0],
      sugars: [0],
      salt: [0],
      image: [null]
    });
  }

  addProduct() {
    if (this.addProductForm.valid) {
      this.isLoading = true;
      const formData = new FormData();
      formData.append('productName', this.addProductForm.get('productName')?.value);
      formData.append('calories', this.addProductForm.get('calories')?.value);
      formData.append('proteins', this.addProductForm.get('proteins')?.value);
      formData.append('carbohydrates', this.addProductForm.get('carbohydrates')?.value);
      formData.append('fats', this.addProductForm.get('fats')?.value);
      formData.append('sugars', this.addProductForm.get('sugars')?.value);
      formData.append('salt', this.addProductForm.get('salt')?.value);

      if (this.selectedFile) {
        formData.append('image', this.selectedFile, this.selectedFile.name);
      }

      this.productService.addProduct(formData).subscribe({
        next: _ => this.router.navigateByUrl('/product'),
        error: error => this.validationErrors = error
      });
    }
  }

  cancel() {
    this.cancelAddProduct.emit(false);
  }

  onFileUpload(event: File) {
    console.log(event);
    this.selectedFile = event;
    this.addProductForm.patchValue({ image: event });
    console.log(this.addProductForm);
  }
}
