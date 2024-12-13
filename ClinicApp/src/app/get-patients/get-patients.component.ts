import { Component, Input } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Patient } from '../model/patient';
import { RouterLink, RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-get-patients',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './get-patients.component.html',
  styleUrl: './get-patients.component.css'
})
export class GetPatientsComponent {
  readonly APIUrl="https://localhost:5001/api/Patient";
  patients: Patient[] = [];
  //medicalAppointmentForm: FormGroup;
  isDisable = false;
  patientForm: FormGroup;
  isVisible: boolean = false;

  @Input() isAddingMode: boolean = false; // Odbiera zmienną od rodzica



  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.patientForm = this.formBuilder.group({});
  }
  
  ngOnInit(){
    this.getAllPatients();
    this.patientForm = this.formBuilder.group({
      id: new  FormControl(0, {validators: [Validators.required]}),
      name: new  FormControl('', {validators: [Validators.required]}),
      surname: new  FormControl('', {validators: [Validators.required]}),
      pesel: new  FormControl('', {validators: [Validators.required]}),
    });
  }

  ngOnChanges() {
    console.log('isAddingMode in GetPatients:', this.isAddingMode);
  }

  get formId(): FormControl {return this.patientForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl {return this.patientForm?.get("name") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSurname(): FormControl {return this.patientForm?.get("surname") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formPesel(): FormControl {return this.patientForm?.get("pesel") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?

  getAllPatients(){
    this.http.get<Patient[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.patients=data;
    })
  }

  /*edit(patient: Patient){
    this.http.put<Patient>(this.APIUrl+"/update", patient)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })
  } */

  edit(patient: Patient){
    this.isVisible = true;  
    this.formId.setValue(patient.id);
    this.formName.setValue(patient.name);
    this.formSurname.setValue(patient.surname);
    this.formPesel.setValue(patient.pesel);

    //this.fillForm(measure);
  }


  update(){
    if(this.patientForm.invalid){
      this.patientForm.markAllAsTouched(); 
      return;
    }
    this.http.put<Patient>(this.APIUrl+"/update", this.patientForm.getRawValue())
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }


  delete(patientId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+patientId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllPatients();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }


}
