import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { DietComponent } from './diet/diet.component';
import { AddProductComponent } from './add-product/add-product.component';
import { DietPlanComponent } from './diet-plan/diet-plan.component';
import { UserProductsComponent } from './user-products/user-products.component';
import { EditProductComponent } from './edit-product/edit-product.component';
import { LearnMoreComponent } from './learn-more/learn-more.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'diet', component: DietComponent},
  {path: 'learn-more', component: LearnMoreComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'product/create', component: AddProductComponent},
      {path: 'diet-plan', component: DietPlanComponent},
      {path: 'diet-plan/:dietPlanId', component: DietPlanComponent},
      {path: 'product', component: UserProductsComponent},
      {path: 'product/:productId', component: EditProductComponent},
    ]
  },
  {path: '**', component: HomeComponent, pathMatch: 'full'}
];
