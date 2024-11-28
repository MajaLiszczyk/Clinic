import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { YourDataComponent } from "./patient/your-data/your-data/your-data.component";
//import { BrowserModule } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, YourDataComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'ClinicApp';
}
