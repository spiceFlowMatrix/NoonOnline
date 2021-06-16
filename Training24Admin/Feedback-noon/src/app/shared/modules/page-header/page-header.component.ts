import { Component, OnInit, Input, wtfLeave } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { UtilService } from './../../services/utils.services';
import { LanguageList } from './../../models/menu-lists';
@Component({
    selector: 'app-page-header',
    templateUrl: './page-header.component.html',
    styleUrls: ['./page-header.component.scss']
})
export class PageHeaderComponent implements OnInit {
    @Input() heading: string;
    public languageList = [];
    constructor(
        public utilService: UtilService,
        public activatedRoute: ActivatedRoute,
        public translate: TranslateService,
    ) {
        this.languageList = LanguageList;
    }

    ngOnInit() {
        this.translate.addLangs(['en', 'fa', 'ps']);
        this.translate.setDefaultLang('en');
        this.activatedRoute.queryParams.subscribe(params => {
            if (params['hl'] && (['en', 'fa', 'ps'].indexOf(params['hl']) > -1)) {
                this.changeLang(params['hl']);
            } else {
                let browserLang = localStorage.getItem("locale");
                if (!browserLang) {
                    browserLang = this.translate.getBrowserLang();
                }
                let langLocale: any = browserLang.match(/en|fa|ps/) ? browserLang : 'en';
                this.utilService.rltAndLtr(langLocale);
                this.translate.use(langLocale);
            }
        })
        // this.translate.addLangs(['en', 'fa', 'ps']);
        // this.translate.setDefaultLang('en');
        // let browserLang = localStorage.getItem("locale");
        // if (!browserLang) {
        //     browserLang = this.translate.getBrowserLang();
        // }
        // let langLocale: any = browserLang.match(/en|fa|ps/) ? browserLang : 'en';
        // this.utilService.rltAndLtr(langLocale);
        // // localStorage.setItem("locale", langLocale);
        // this.translate.use(langLocale);
    }

    changeLang(language: string) {
        this.translate.use(language);
        this.utilService.rltAndLtr(language);
        localStorage.setItem("locale", language);
    }
}
