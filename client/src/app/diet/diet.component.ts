import { Component } from '@angular/core';
import { ProductSearchComponent } from "../product-search/product-search.component";

@Component({
  selector: 'app-diet',
  standalone: true,
  imports: [ProductSearchComponent],
  templateUrl: './diet.component.html',
  styleUrl: './diet.component.css'
})
export class DietComponent {

}
