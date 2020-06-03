import { Component, OnInit } from '@angular/core';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { DeviceService } from '../../../shared/services/device.service';

const ELEMENT_DATA: any[] = [
  {
    'Current Quota Limitation': 1,
    Username: 'Hydrogen',
    'Email Address': 1.0079,
    'Active Devices Number': 'H',
    description: `Hydrogen is a chemical element with 'Active Devices Number' H and atomic number 1. With a standard
        atomic Email Address of 1.008, hydrogen is the lightest element on the periodic table.`
  }, {
    'Current Quota Limitation': 2,
    Username: 'Helium',
    'Email Address': 4.0026,
    'Active Devices Number': 'He',
    description: `Helium is a chemical element with 'Active Devices Number' He and atomic number 2. It is a
        colorless, odorless, tasteless, non-toxic, inert, monatomic gas, the first in the noble gas
        group in the periodic table. Its boiling point is the lowest among all the elements.`
  }, {
    'Current Quota Limitation': 3,
    Username: 'Lithium',
    'Email Address': 6.941,
    'Active Devices Number': 'Li',
    description: `Lithium is a chemical element with 'Active Devices Number' Li and atomic number 3. It is a soft,
        silvery-white alkali metal. Under standard conditions, it is the lightest metal and the
        lightest solid element.`
  }, {
    'Current Quota Limitation': 4,
    Username: 'Beryllium',
    'Email Address': 9.0122,
    'Active Devices Number': 'Be',
    description: `Beryllium is a chemical element with 'Active Devices Number' Be and atomic number 4. It is a
        relatively rare element in the universe, usually occurring as a product of the spallation of
        larger atomic nuclei that have collided with cosmic rays.`
  }, {
    'Current Quota Limitation': 5,
    Username: 'Boron',
    'Email Address': 10.811,
    'Active Devices Number': 'B',
    description: `Boron is a chemical element with 'Active Devices Number' B and atomic number 5. Produced entirely
        by cosmic ray spallation and supernovae and not by stellar nucleosynthesis, it is a
        low-abundance element in the Solar system and in the Earth's crust.`
  }, {
    'Current Quota Limitation': 6,
    Username: 'Carbon',
    'Email Address': 12.0107,
    'Active Devices Number': 'C',
    description: `Carbon is a chemical element with anumber C and atomic number 6. It is nonmetallic
        and tetravalent—making four electrons available to form covalent chemical bonds. It belongs
        to group 14 of the periodic table.`
  }, {
    'Current Quota Limitation': 7,
    Username: 'Nitrogen',
    'Email Address': 14.0067,
    'Active Devices Number': 'N',
    description: `Nitrogen is a chemical element with symbol N and atomic number 7. It was first
        discovered and isolated by Scottish physician Daniel Rutherford in 1772.`
  }];
const ELEMENT_DATA2: any[] = [
  { Date: 1, Username: 'Hydrogen', 'Device Id': 1.0079, 'Email Address': 'H', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 2, Username: 'Helium', 'Device Id': 4.0026, 'Email Address': 'He', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 3, Username: 'Lithium', 'Device Id': 6.941, 'Email Address': 'Li', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 4, Username: 'Beryllium', 'Device Id': 9.0122, 'Email Address': 'Be', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 5, Username: 'Boron', 'Device Id': 10.811, 'Email Address': 'B', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 6, Username: 'Carbon', 'Device Id': 12.0107, 'Email Address': 'C', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 7, Username: 'Nitrogen', 'Device Id': 14.0067, 'Email Address': 'N', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 8, Username: 'Oxygen', 'Device Id': 15.9994, 'Email Address': 'O', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 9, Username: 'Fluorine', 'Device Id': 18.9984, 'Email Address': 'F', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
  { Date: 10, Username: 'Neon', 'Device Id': 20.1797, 'Email Address': 'Ne', 'Current Quota Limitation': 2, 'Requested Quota Limistation': 5, Status: 'Pending' },
];
@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class DeviceComponent implements OnInit {
  dataSource = ELEMENT_DATA;
  columnsToDisplay = ['username', 'email', 'currentConsumption', 'deviceLimit'];
  // columnsToDisplay = [
  //   { display: 'username', use: 'Username' },
  //   { display: 'email', use: 'Email Address' },
  //   { display: 'currentConsumption', use: 'Current Quota Limitation' },
  //   { display: 'deviceLimit', use: 'Active Device Number' }];
  expandedElement: any;
  displayedColumns: string[] = ['Date', 'Username', 'Email Address', 'Device Id', 'Current Quota Limitation', 'Requested Quota Limistation', 'Status'];
  dataSourceMini = ELEMENT_DATA2;

  userEmails: any;
  deviceModels: any;
  devices: any;
  filterModal:any = {
    page: 1,
    pageRecord:10,
    userId:0,
    search:''
  }
  constructor(public deviceService: DeviceService) { }

  ngOnInit(): void {
    this.getDevice();
  }
  getDevice() {
    this.deviceService.getDevice(this.filterModal).subscribe((res) => {
      console.log(res);
      this.userEmails = res.data.userEmails;
      this.dataSource = res.data.userDeviceModels;
    })
  }
  getUserDevice(id) {
    this.devices = null;
    this.deviceService.getUserDevice(id).subscribe((res) => {
      console.log(res);
      this.devices = res.data;
    })
  }
  toggleStatus(id,i,userId) {
    console.log(id);
    this.deviceService.toggleDeviceStatus(id,userId).subscribe((res)=> {
      this.devices[i].isActive =! this.devices[i].isActive;
      console.log(res);
    })
    
  }
}
