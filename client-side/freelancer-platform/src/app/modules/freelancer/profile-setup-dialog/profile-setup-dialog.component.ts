import { Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FreelancerService } from '../services/freelancer.service';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HourlyRate, ProfileSummary } from '../models/freelancer.model';
import { Language, LanguageKnowledge } from '../../shared/models/language.model';
import { Profession } from '../../shared/models/profession.mode';
import { ExperienceLevel } from '../../shared/models/experience-level.model';
import { LanguageService } from '../../shared/services/language.service';
import { ProfessionService } from '../../shared/services/profession.service';
import { AuthService } from '../../auth/services/auth.service';
import { ProfileSetupCommand } from '../models/commands/profile-setup-command.model';
import {Role} from "../../shared/models/role.model";
import {Contact} from "../../shared/models/contact.model";

@Component({
  selector: 'app-profile-setup-dialog',
  templateUrl: './profile-setup-dialog.component.html',
  styleUrls: ['./profile-setup-dialog.component.scss'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false, showError: true},
    },
  ],
})
export class CompleteRegisterDialogComponent {

  isCompleted: boolean = false;

  languages: Language[] = [];
  professions: Profession[] = [];
  contact: Contact = new Contact();
  role: Role = Role.Freelancer;

  generalFormGroup = this.formBuilder.group({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    contact: this.formBuilder.group({
      phoneNumber: new FormControl('', Validators.required),
      address: this.formBuilder.group({
        country: new FormControl('', Validators.required),
        city: new FormControl('', Validators.required),
        street: new FormControl('', Validators.required),
        number: new FormControl('', Validators.required),
        zipCode: new FormControl('', Validators.required),
      })
    }),
    isProfilePublic: new FormControl(true, Validators.required),
    availability: new FormControl(0, Validators.required),
  });

  professionGroup = this.formBuilder.group({
    profession: [this.professions[0], Validators.required],
    experienceLevel: [0, Validators.required]
  });

  profileSummaryGroup = this.formBuilder.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
  });

  languageKnowledgeGroup = this.formBuilder.group({
    language: [this.languages[0], Validators.required],
    profiencyLevel: [0, Validators.required]
  });

  hourlyRateGroup = this.formBuilder.group({
    amount: [0.0, Validators.required],
    currency: ['EUR', Validators.required]
  });

  availabilityGroup = this.formBuilder.group({
    availability: [0, Validators.required],
    profileType: [false, Validators.required]
  });

  constructor(
    private dialogRef: MatDialogRef<CompleteRegisterDialogComponent>,
    private authService: AuthService,
    private freelancerService: FreelancerService,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private languageService: LanguageService,
    private professionService: ProfessionService)
  {
    this.languageService.get().subscribe((languages) => this.languages = languages);
    this.professionService.get().subscribe((professions) => this.professions = professions);

    this.dialogRef.afterClosed().subscribe(() => {
      if (!this.isCompleted)
        CompleteRegisterDialogComponent.open(this.dialog);
    });

  }

  parseToCommandModel(): ProfileSetupCommand {
    var command = this.generalFormGroup.value as ProfileSetupCommand;
    command.hourlyRate = this.hourlyRateGroup.value as HourlyRate;
    command.profileSummary = this.profileSummaryGroup.value as ProfileSummary;
    let languageKnowledge = this.languageKnowledgeGroup.value as LanguageKnowledge;
    command.languageId = languageKnowledge.language.id;
    command.languageProficiencyLevel = languageKnowledge.profiencyLevel;
    command.professionId = (this.professionGroup.value.profession as Profession).id;
    command.experienceLevel =  this.professionGroup.value.experienceLevel as ExperienceLevel;
    return command;
  }

  setupProfile(): void {
    var createCommand = this.parseToCommandModel();
    console.log(createCommand)
    this.freelancerService.setupProfile(createCommand).subscribe({
      complete: () => {
        this.isCompleted = true;
        this.dialogRef.close();
      },
      error: () => console.log('error')
    });
  }

  static open(dialog: MatDialog): MatDialogRef<CompleteRegisterDialogComponent> {
    return dialog.open(CompleteRegisterDialogComponent, {
      width: '40%',
      height: '65%'
    });
  }

}
