export interface Product {
  productId: number;
  productName: string;
  calories?: number;
  fats?: number;
  carbohydrates?: number;
  sugars?: number;
  proteins?: number;
  salt?: number;
  imageUrl?: string;
  weight: number;
}
