import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetRegistrantsComponent } from './get-registrants.component';

describe('GetRegistrantsComponent', () => {
  let component: GetRegistrantsComponent;
  let fixture: ComponentFixture<GetRegistrantsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetRegistrantsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetRegistrantsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
