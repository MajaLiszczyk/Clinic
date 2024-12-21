import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentDetailsOldComponent } from './appointment-details-old.component';

describe('AppointmentDetailsOldComponent', () => {
  let component: AppointmentDetailsOldComponent;
  let fixture: ComponentFixture<AppointmentDetailsOldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppointmentDetailsOldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AppointmentDetailsOldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
