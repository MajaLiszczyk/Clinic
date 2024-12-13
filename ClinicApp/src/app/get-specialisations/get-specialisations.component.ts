import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';

@Component({
  selector: 'app-get-specialisations',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-specialisations.component.html',
  styleUrl: './get-specialisations.component.css'
})
export class GetSpecialisationsComponent {
  readonly APIUrl="https://localhost:5001/api/MedicalSpecialisation";
  specialisations: Specialisation[] = [];
  specialisationForm: FormGroup;
  isDisable = false;
  isVisible: boolean = false;
  //medicalAppointmentForm: FormGroup;


  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.specialisationForm = this.formBuilder.group({});
  }
  
  ngOnInit(){
    this.getAllSpecialisations();
    this.specialisationForm = this.formBuilder.group({
      name: new  FormControl('', {validators: [Validators.required]}),
      id: new  FormControl(0, {validators: [Validators.required]}),
    });
    
  }

  get formName(): FormControl {return this.specialisationForm?.get("name") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formId(): FormControl {return this.specialisationForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?


    /*get formId(): FormControl {return this.medicalAppointmentForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formPatientId(): FormControl {return this.medicalAppointmentForm?.get("patientId") as FormControl};
    get formDoctorId(): FormControl {return this.medicalAppointmentForm?.get("doctorId") as FormControl};
    get formMedicalAppointmentDate(): FormControl {return this.medicalAppointmentForm.get("dateTime") as FormControl};
    get timeControl(): FormControl {return this.medicalAppointmentForm.get("time") as FormControl};*/

  getAllSpecialisations(){
    this.http.get<Specialisation[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.specialisations=data;
    })

  }


  edit(specialisation: Specialisation){
    this.isVisible = true;  
    /*this.specialisationForm.reset();
    this.editableMeasure = measure;
    this.isFormVisible = true;
    this.editableMode = true;
    this.operationResult = null;*/
    this.formId.setValue(specialisation.id);
    this.formName.setValue(specialisation.name);

    //this.fillForm(measure);
  }


  update(){
    if(this.specialisationForm.invalid){
      this.specialisationForm.markAllAsTouched(); 
      return;
    }
    this.http.put<Specialisation>(this.APIUrl+"/update", this.specialisationForm.getRawValue())
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(dspecialisationId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+ dspecialisationId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllSpecialisations();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

}
