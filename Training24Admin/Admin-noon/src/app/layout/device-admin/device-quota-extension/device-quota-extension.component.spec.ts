import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceQuotaExtensionComponent } from './device-quota-extension.component';

describe('DeviceQuotaExtensionComponent', () => {
  let component: DeviceQuotaExtensionComponent;
  let fixture: ComponentFixture<DeviceQuotaExtensionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeviceQuotaExtensionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeviceQuotaExtensionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
