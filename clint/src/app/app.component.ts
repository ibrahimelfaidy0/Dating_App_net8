import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',


  standalone: true,
  imports: [CommonModule, FormsModule],
   
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit  {
  http = inject(HttpClient);
  errormsg: string = ''; // Corrected variable casing
  users: any;

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        // Extract error message from the response
        this.errormsg = err.message || 'An error occurred while fetching users.';
      },
      complete: () => console.log('HTTP request completed'),
    });
  }
  }

   




