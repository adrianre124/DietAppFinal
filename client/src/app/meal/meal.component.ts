import { Component, Input, OnInit, inject, input, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Meal } from '../_models/meal';
import { DietPlanService } from '../_services/diet-plan.service';
import { Product } from '../_models/product';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-meal',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent, NgIf, NgFor],
  templateUrl: './meal.component.html',
  styleUrl: './meal.component.css'
})
export class MealComponent implements OnInit{
  private fb = inject(FormBuilder);
  mealForm: FormGroup = new FormGroup({});
  currentMeal: Meal | null = null;
  addedMeal = output<Meal>();

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.mealForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  addMeal(): void {
    if (this.mealForm.valid) {
      this.addedMeal.emit(this.mealForm.value);
      // this.mealForm.setValue({name: ''}, {emitEvent: false});
    }
  }
}
