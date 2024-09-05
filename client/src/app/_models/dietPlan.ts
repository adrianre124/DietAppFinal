import { Meal } from "./meal";

export interface DietPlan {
  dietPlanId: number;
  name: string;
  createDate: Date;
  meals: Meal[];
}
