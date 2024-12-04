import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { YourDataComponent } from "./your-data/your-data.component";
//import { BrowserModule } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, YourDataComponent, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  template:  `
  <div class="container">
    <h1>Welcome to the App!</h1>
    <router-outlet></router-outlet>
  </div>
`,
})
export class AppComponent {
  title = 'ClinicApp';
}
