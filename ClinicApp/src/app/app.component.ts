import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from "./log-in/log-in.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, HttpClientModule, LoginComponent],
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
