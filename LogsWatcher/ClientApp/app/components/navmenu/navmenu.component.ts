import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    username: any;
    constructor(http: Http) {
        var that = this;
        http.get('http://localhost:61148/Home/GetUserName').subscribe(result => {
          if (result) {
            that.username = (<any>result)._body;
          }
        }, error => console.error(error));
    }
}
