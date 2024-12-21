import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantDiagnosticTestTypesComponent } from './registrant-diagnostic-test-types.component';

describe('RegistrantDiagnosticTestTypesComponent', () => {
  let component: RegistrantDiagnosticTestTypesComponent;
  let fixture: ComponentFixture<RegistrantDiagnosticTestTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantDiagnosticTestTypesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantDiagnosticTestTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
