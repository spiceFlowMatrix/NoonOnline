import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UtilService, AuthenticationService, MenuList, DataService, SalesMenuList } from '../../../shared';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
    isActive: boolean = false;
    collapsed: boolean = false;
    showMenu: string = '';
    pushRightClass: string = 'push-right';
    menuList: any = [];
    roles: any;

    @Output() collapsedEvent = new EventEmitter<boolean>();

    constructor(
        private translate: TranslateService,
        public utilService: UtilService,
        public authenticationService: AuthenticationService,
        public dataService: DataService,
        public router: Router) {
        this.translate.addLangs(['en', 'fr', 'ur', 'es', 'it', 'fa', 'de']);
        this.translate.setDefaultLang('en');
        const browserLang = this.translate.getBrowserLang();
        this.translate.use(browserLang.match(/en|fr|ur|es|it|fa|de/) ? browserLang : 'en');

        this.router.events.subscribe(val => {
            if (
                val instanceof NavigationEnd &&
                window.innerWidth <= 992 &&
                this.isToggled()
            ) {
                this.toggleSidebar();
            }
        });
    }

    // ngOnInit() {
    //     this.menuList = AdminSalesMenulist;       
    //     this.dataService.sideMenu$.subscribe(res => {
    //         setTimeout(() => {
    //             this.collapsed = res;
    //             this.collapsedEvent.emit(this.collapsed);
    //         }, 200);
    //     });
    // }
    ngOnInit() {
        this.menuList = [];
     this.roles = this.utilService.getRole();
      
      if(this.roles.length > 1){
        if((this.utilService.getRole().indexOf('sales_agent') > -1) && (this.utilService.getRole().indexOf('sales_admin') > -1))
        {
            this.menuList = MenuList;
        }   
        if((this.utilService.getRole().indexOf('sales_agent') > -1) && (this.utilService.getRole().indexOf('admin') > -1))
        {
            this.menuList = MenuList;
        }   
        if(this.utilService.getRole().indexOf('admin') > -1 && this.utilService.getRole().indexOf('sales_admin') > -1)
        {
            this.menuList = MenuList;
        } 
      }
      else {
        if (this.utilService.getRole().indexOf('admin') > -1)
        {
            this.menuList = MenuList;
        }
        if (this.utilService.getRole().indexOf('sales_admin') > -1)
        {
            this.menuList = MenuList;
        }
        if(this.utilService.getRole().indexOf('sales_agent') > -1)
        {
            this.menuList = SalesMenuList;
        }        
      }
        this.dataService.sideMenu$.subscribe(res => {
            setTimeout(() => {
                this.collapsed = res;
                this.collapsedEvent.emit(this.collapsed);
            }, 300);
        });
    }

    eventCalled() {
        this.isActive = !this.isActive;
    }

    addExpandClass(element: any) {
        if (element === this.showMenu) {
            this.showMenu = '0';
        } else {
            this.showMenu = element;
        }
    }

    toggleCollapsed() {
        this.collapsed = !this.collapsed;
        this.collapsedEvent.emit(this.collapsed);
    }

    isToggled(): boolean {
        const dom: Element = document.querySelector('body');
        return dom.classList.contains(this.pushRightClass);
    }

    toggleSidebar() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle(this.pushRightClass);
    }

    rltAndLtr() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle('rtl');
    }

    changeLang(language: string) {
        this.translate.use(language);
    }

    onLoggedout() {
        this.authenticationService.logout();
    }
}
