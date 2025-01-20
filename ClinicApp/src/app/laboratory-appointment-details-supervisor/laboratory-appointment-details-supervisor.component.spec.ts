import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaboratoryAppointmentDetailsSupervisorComponent } from './laboratory-appointment-details-supervisor.component';

describe('LaboratoryAppointmentDetailsSupervisorComponent', () => {
  let component: LaboratoryAppointmentDetailsSupervisorComponent;
  let fixture: ComponentFixture<LaboratoryAppointmentDetailsSupervisorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaboratoryAppointmentDetailsSupervisorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LaboratoryAppointmentDetailsSupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
