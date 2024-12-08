import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientParentComponent } from './patient-parent.component';

describe('PatientParentComponent', () => {
  let component: PatientParentComponent;
  let fixture: ComponentFixture<PatientParentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientParentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PatientParentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
