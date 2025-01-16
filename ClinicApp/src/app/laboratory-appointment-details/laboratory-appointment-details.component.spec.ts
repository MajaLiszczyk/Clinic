import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaboratoryAppointmentDetailsComponent } from './laboratory-appointment-details.component';

describe('LaboratoryAppointmentDetailsComponent', () => {
  let component: LaboratoryAppointmentDetailsComponent;
  let fixture: ComponentFixture<LaboratoryAppointmentDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaboratoryAppointmentDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LaboratoryAppointmentDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
