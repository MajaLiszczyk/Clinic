import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantLaboratoryTestTypesComponent } from './registrant-laboratory-test-types.component';

describe('RegistrantLaboratoryTestTypesComponent', () => {
  let component: RegistrantLaboratoryTestTypesComponent;
  let fixture: ComponentFixture<RegistrantLaboratoryTestTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantLaboratoryTestTypesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantLaboratoryTestTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
