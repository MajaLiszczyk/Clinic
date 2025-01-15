import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaboratoryWorkerComponent } from './laboratory-worker.component';

describe('LaboratoryWorkerComponent', () => {
  let component: LaboratoryWorkerComponent;
  let fixture: ComponentFixture<LaboratoryWorkerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaboratoryWorkerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LaboratoryWorkerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
