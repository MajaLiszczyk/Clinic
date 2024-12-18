import { Component } from '@angular/core';
import { AddSpecialisationComponent } from "../add-specialisation/add-specialisation.component";
import { GetSpecialisationsComponent } from "../get-specialisations/get-specialisations.component";
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-registrant-specialisations-old',
  standalone: true,
  imports: [AddSpecialisationComponent, GetSpecialisationsComponent, CommonModule, RouterLink],
  templateUrl: './registrant-specialisations-old.component.html',
  styleUrl: './registrant-specialisations-old.component.css'
})
export class RegistrantSpecialisationsOldComponent {
  isAddNewSpecialisationVisible: boolean = false;

  addNewSpecialisation(){
    this.isAddNewSpecialisationVisible = true;
  }

}
