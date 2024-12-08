import { Component } from '@angular/core';
import { AddSpecialisationComponent } from "../add-specialisation/add-specialisation.component";
import { GetSpecialisationsComponent } from "../get-specialisations/get-specialisations.component";

@Component({
  selector: 'app-registrant-specialisations',
  standalone: true,
  imports: [AddSpecialisationComponent, GetSpecialisationsComponent],
  templateUrl: './registrant-specialisations.component.html',
  styleUrl: './registrant-specialisations.component.css'
})
export class RegistrantSpecialisationsComponent {
  isAddNewSpecialisationVisible: boolean = false;

  addNewSpecialisation(){
    this.isAddNewSpecialisationVisible = true;
  }

}
