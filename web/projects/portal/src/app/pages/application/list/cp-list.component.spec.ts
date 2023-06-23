import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CpListComponent } from './cp-list.component';

describe('CpListComponent', () => {
  let component: CpListComponent;
  let fixture: ComponentFixture<CpListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CpListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CpListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
