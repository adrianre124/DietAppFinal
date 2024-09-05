import { Component, inject, input, output } from '@angular/core';
import { Product } from '../_models/product';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../_services/product.service';
import { ToastrService } from 'ngx-toastr';
import { NgIf } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, TextInputComponent, PhotoEditorComponent],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css'
})
export class EditProductComponent {
  private productService = inject(ProductService);
  private fb = inject(FormBuilder);
  private toastr = inject(ToastrService);
  private route = inject(ActivatedRoute);
  product = input<Product | null>(null);
  editToggle = output<boolean>();
  editProductForm: FormGroup = new FormGroup([]);
  selectedFile: File | null = null;

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.editProductForm = this.fb.group({
      productName: [this.product()?.productName, Validators.required],
      calories: [this.product()?.calories, Validators.required],
      fats: [this.product()?.fats, Validators.required],
      carbohydrates: [this.product()?.carbohydrates, Validators.required],
      sugars: [this.product()?.sugars, Validators.required],
      proteins: [this.product()?.proteins, Validators.required],
      salt: [this.product()?.salt, Validators.required],
      image: [null]
    });
  }

  onSubmit(id?: number): void {
    if (this.editProductForm.valid) {
      const formData = new FormData();
      formData.append('productName', this.editProductForm.get('productName')?.value);
      formData.append('calories', this.editProductForm.get('calories')?.value);
      formData.append('proteins', this.editProductForm.get('proteins')?.value);
      formData.append('carbohydrates', this.editProductForm.get('carbohydrates')?.value);
      formData.append('fats', this.editProductForm.get('fats')?.value);
      formData.append('sugars', this.editProductForm.get('sugars')?.value);
      formData.append('salt', this.editProductForm.get('salt')?.value);

      if (this.selectedFile) {
        formData.append('image', this.selectedFile, this.selectedFile.name);
      }
      console.log(this.product()?.productId);
      console.log(formData);
      if (this.product()?.productId !== undefined) {
        this.productService.updateProduct(this.product()?.productId!, formData).subscribe({
          next: () => {
            this.toastr.success('Product updated', 'Success');
            window.location.reload();
          },
          error: () => this.toastr.error('Failed updating product', 'Error')
        });
      }
    }
  }

  toggleEdit() {
    this.editToggle.emit(false);
  }

  onFileUpload($event: File) {
    this.selectedFile = $event;
    this.editProductForm.patchValue({ image: $event})
  }
}
