import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMedicalAppointmentComponent } from './add-medical-appointment.component';

describe('AddMedicalAppointmentComponent', () => {
  let component: AddMedicalAppointmentComponent;
  let fixture: ComponentFixture<AddMedicalAppointmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddMedicalAppointmentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddMedicalAppointmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
