import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { YourDataComponent } from "./your-data/your-data.component";
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from "./log-in/log-in.component";
//import { ClinicService } from './services/clinic.service';
//import { BrowserModule } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, YourDataComponent, RouterModule, HttpClientModule, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  template:  `
  <div class="container">
    <h1>Welcome to the App!</h1>
    <router-outlet></router-outlet>
  </div>
`,
  //providers: [ClinicService],
})
export class AppComponent {
  title = 'ClinicApp';
  //constructor(private clinicService: ClinicService){}
}
