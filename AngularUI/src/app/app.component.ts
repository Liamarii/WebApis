import { Component, AfterViewInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
})
export class AppComponent implements AfterViewInit {
  title = 'AngularUI';

  ngAfterViewInit() {
    document.body.setAttribute('data-testid', 'app-ready');
  }
}
