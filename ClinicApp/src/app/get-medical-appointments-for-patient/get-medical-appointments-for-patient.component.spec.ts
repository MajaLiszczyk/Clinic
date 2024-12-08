import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetMedicalAppointmentsForPatientComponent } from './get-medical-appointments-for-patient.component';

describe('GetMedicalAppointmentsForPatientComponent', () => {
  let component: GetMedicalAppointmentsForPatientComponent;
  let fixture: ComponentFixture<GetMedicalAppointmentsForPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetMedicalAppointmentsForPatientComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetMedicalAppointmentsForPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
