import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetMedicalAppointmentsComponent } from './get-medical-appointments.component';

describe('GetMedicalAppointmentsComponent', () => {
  let component: GetMedicalAppointmentsComponent;
  let fixture: ComponentFixture<GetMedicalAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetMedicalAppointmentsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetMedicalAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
