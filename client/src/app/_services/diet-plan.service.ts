import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { DietPlan } from '../_models/dietPlan';
import { Meal } from '../_models/meal';

@Injectable({
  providedIn: 'root'
})
export class DietPlanService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  createDietPlan(name: any) {
    return this.http.post<DietPlan>(this.baseUrl + 'dietplan', name);
  }

  getDietPlan(id: number) {
    return this.http.get<DietPlan>(`${this.baseUrl}dietplan/${id}`);
  }

  getDietPlans() {
    return this.http.get<DietPlan[]>(this.baseUrl + 'dietplan');
  }

  addMeal(dietPlanId: number, mealData: Meal) {
    return this.http.post<Meal>(this.baseUrl + 'dietplan/' + dietPlanId + '/add-meal', mealData);
  }

  updateDietPlan(dietPlan: DietPlan) {
    return this.http.put(this.baseUrl + 'dietplan/' + dietPlan.dietPlanId, dietPlan);
  }

  removeDietPlan(id: number) {
    return this.http.delete(this.baseUrl + 'dietplan/delete/' + id);
  }
}
