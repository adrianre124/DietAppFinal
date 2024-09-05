import { Product } from "./product";

export interface Meal {
  mealId: number;
  name: string;
  products: Product[];
}
