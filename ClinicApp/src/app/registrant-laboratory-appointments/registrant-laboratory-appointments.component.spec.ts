import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantLaboratoryAppointmentsComponent } from './registrant-laboratory-appointments.component';

describe('RegistrantLaboratoryAppointmentsComponent', () => {
  let component: RegistrantLaboratoryAppointmentsComponent;
  let fixture: ComponentFixture<RegistrantLaboratoryAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantLaboratoryAppointmentsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantLaboratoryAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
