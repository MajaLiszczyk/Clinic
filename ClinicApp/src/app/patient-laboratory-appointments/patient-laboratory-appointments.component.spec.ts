import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientLaboratoryAppointmentsComponent } from './patient-laboratory-appointments.component';

describe('PatientLaboratoryAppointmentsComponent', () => {
  let component: PatientLaboratoryAppointmentsComponent;
  let fixture: ComponentFixture<PatientLaboratoryAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientLaboratoryAppointmentsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PatientLaboratoryAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
