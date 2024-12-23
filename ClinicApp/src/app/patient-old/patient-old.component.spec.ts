import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientOldComponent } from './patient-old.component';

describe('PatientOldComponent', () => {
  let component: PatientOldComponent;
  let fixture: ComponentFixture<PatientOldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientOldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PatientOldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
