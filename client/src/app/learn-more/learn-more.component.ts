import { Component, output } from '@angular/core';

@Component({
  selector: 'app-learn-more',
  standalone: true,
  imports: [],
  templateUrl: './learn-more.component.html',
  styleUrl: './learn-more.component.css'
})
export class LearnMoreComponent {
  toggleLearnMoreMode = output<boolean>();

  toggleLeanMore() {
    this.toggleLearnMoreMode.emit(false);
  }
}
