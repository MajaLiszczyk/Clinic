using ClinicAPI.Dtos;

namespace ClinicAPI.Models
{
    public class MedicalAppointmentsOfPatient
    {
        public List<ReturnMedicalAppointmentDto> pastMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentDto>();
        public List<ReturnMedicalAppointmentDto> futureMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentDto>();
    }
}


/*
 * Kontroler przy get zwraca obiekt tej klasy. Nie 
  public class MedicalAppointmentsOfPatient
    {
        public List<ReturnMedicalAppointmentDto> pastMedicalAppointments;
        public List<ReturnMedicalAppointmentDto> futureMedicalAppointments;
    }


Frontend:

  allMedicalAppointments: AllMedicalAppointments;
  isDisable = false;
  patients: Patient[]= [];
  selectedPatientId: number = 0;
  choosePatientForm: FormGroup;



  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.choosePatientForm = this.formBuilder.group({});
    this.allMedicalAppointments = {pastMedicalAppointments: [], futureMedicalAppointments: []}

  }

getAllMedicalAppointments(patientId:number){
    this.http.get<AllMedicalAppointments>(this.APIUrl+"/GetByPatientId/" + patientId).subscribe(data =>{
      this.allMedicalAppointments=data;
    })
  }

export interface AllMedicalAppointments{
    pastMedicalAppointments: MedicalAppointment[];
    futureMedicalAppointments: MedicalAppointment[];
}


<div class="d-flex justify-content-center mx-5 px-5 mb-5">
    <table class="table table-striped">
        <thead class="thead-dark">
            <tr>
                <th>
                    Past appointments:
                </th>
                <th>
                   
                </th>
            </tr>

        </thead>
        <tr *ngFor="let pastAppointment of allMedicalAppointments.pastMedicalAppointments">
            <td>
                Time: {{pastAppointment.dateTime}}
                Doctor Id: {{pastAppointment.doctorId}}
            </td>
        </tr>
        <tr *ngIf="patients.length === 0">
            <td colspan="8">No past medical appointments</td>
        </tr>
    </table>
</div>

<div class="d-flex justify-content-center mx-5 px-5 mb-5">
    <table class="table table-striped">
        <thead class="thead-dark">
            <tr>
                <th>
                    Future appointments:
                </th>
                <th>
                   
                </th>
            </tr>

        </thead>
        <tr *ngFor="let futureAppointment of allMedicalAppointments.futureMedicalAppointments">
            <td>
                Time: {{futureAppointment.dateTime}}
                Doctor Id: {{futureAppointment.doctorId}}
            </td>
            <td>
                <button type="button" class="btn  btn-success float-right" (click)="cancel(futureAppointment)">Cancel</button>
            </td>
        </tr>
        <tr *ngIf="patients.length === 0">
            <td colspan="8">No future medical appointments</td>
        </tr>
    </table>
</div>



Przy       console.log(this.allMedicalAppointments.pastMedicalAppointments); na frontencie, wypisuje się 'undefined'

 */