import { Component } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { NgIf } from '@angular/common';
import { LearnMoreComponent } from '../learn-more/learn-more.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent, LearnMoreComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  registerMode = false;
  learnMoreMode = false;

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  learnMoreToggle() {
    this.learnMoreMode = !this.learnMoreMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  cancelLearnMoreMode(event: boolean) {
    this.learnMoreMode = event;
  }
}
