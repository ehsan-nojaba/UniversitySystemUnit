# نقشه فارسی کلاس‌ها و فایل‌ها

این فهرست از Summary فارسی سورس تهیه شده است. مسیر هر فایل قابل کلیک است. نام‌های انگلیسی در ستون اول همان نام‌های کد هستند. برای درک فرایند، ابتدا [راهنمای پروژه](PROJECT_GUIDE.fa.md) را بخوانید.

مدل Domain اطلاعات واقعی و قواعد داخلی را نگه می‌دارد. DTO و مدل خروجی Repository فقط داده منتقل می‌کنند و جدول جداگانه نیستند. Configuration نگاشت موجودیت به جدول را تعریف می‌کند. فایل‌های میگریشن موجود، تاریخچه اسکیمای قبلی هستند و در این مرحله دست‌نخورده باقی مانده‌اند.

## Api / Contracts

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AssignProfessorRequest` | بدنه HTTP تخصیص استاد؛ شناسه استاد را دریافت می‌کند و شناسه ارائه از مسیر تعیین می‌شود. | [AssignProfessorRequest.cs](../src/UniversitySystem.Api/Contracts/AssignProfessorRequest.cs) |
| `CreateCourseOfferingRequest` | بدنه HTTP ایجاد ارائه؛ شناسه ترم و درس و ظرفیت را دریافت می‌کند. | [CreateCourseOfferingRequest.cs](../src/UniversitySystem.Api/Contracts/CreateCourseOfferingRequest.cs) |
| `SaveCourseOfferingScheduleRequest` | بدنه HTTP ذخیره برنامه ارائه؛ فهرست روز و بازه‌های کلاس را در Slots نگه می‌دارد. | [SaveCourseOfferingScheduleRequest.cs](../src/UniversitySystem.Api/Contracts/SaveCourseOfferingScheduleRequest.cs) |
| `SaveProfessorAvailabilityRequest` | بدنه HTTP ثبت بازه‌های آزاد استاد؛ روز هفته و زمان شروع و پایان هر بازه را دریافت می‌کند. | [SaveProfessorAvailabilityRequest.cs](../src/UniversitySystem.Api/Contracts/SaveProfessorAvailabilityRequest.cs) |
| `SaveProfessorTeachingRequestRequest` | بدنه HTTP انتخاب درس‌های پیشنهادی استاد؛ شامل شناسه درس و اولویت تدریس است. | [SaveProfessorTeachingRequestRequest.cs](../src/UniversitySystem.Api/Contracts/SaveProfessorTeachingRequestRequest.cs) |
| `SaveStudentPreRegistrationRequest` | بدنه HTTP ذخیره پیش‌انتخاب؛ فهرست شناسه درس و اولویت دانشجو را دریافت می‌کند. | [SaveStudentPreRegistrationRequest.cs](../src/UniversitySystem.Api/Contracts/SaveStudentPreRegistrationRequest.cs) |
| `UpdateCourseOfferingRequest` | بدنه HTTP ویرایش ارائه؛ ظرفیت و وضعیت فعالیت اختیاری را دریافت می‌کند. | [UpdateCourseOfferingRequest.cs](../src/UniversitySystem.Api/Contracts/UpdateCourseOfferingRequest.cs) |

## Api / Controllers

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AdminCourseOfferingsController` | ورودی HTTP بخش «ارائه درس، تخصیص استاد و برنامه زمانی کلاس»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [AdminCourseOfferingsController.cs](../src/UniversitySystem.Api/Controllers/AdminCourseOfferingsController.cs) |
| `AdminPlanningController` | ورودی HTTP بخش «نمای برنامه‌ریزی آموزش»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [AdminPlanningController.cs](../src/UniversitySystem.Api/Controllers/AdminPlanningController.cs) |
| `AdminPreRegistrationController` | ورودی HTTP بخش «تقاضای دانشجوها برای درس‌های یک ترم»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [AdminPreRegistrationController.cs](../src/UniversitySystem.Api/Controllers/AdminPreRegistrationController.cs) |
| `AdminReportsController` | ورودی HTTP بخش «گزارش ظرفیت، ثبت‌نام و درس‌های بدون ارائه»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [AdminReportsController.cs](../src/UniversitySystem.Api/Controllers/AdminReportsController.cs) |
| `AdminTeachingRequestsController` | ورودی HTTP بخش «مشاهده درخواست‌های تدریس ارسال‌شده استادها»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [AdminTeachingRequestsController.cs](../src/UniversitySystem.Api/Controllers/AdminTeachingRequestsController.cs) |
| `ApiControllerBase` | کلاس پایه مشترک کنترلرهای API؛ محل ویژگی‌های مشترک HTTP است. | [ApiControllerBase.cs](../src/UniversitySystem.Api/Controllers/ApiControllerBase.cs) |
| `AuthController` | ورودی HTTP بخش «ورود و دریافت توکن»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [AuthController.cs](../src/UniversitySystem.Api/Controllers/AuthController.cs) |
| `ProfessorTeachingRequestsController` | ورودی HTTP بخش «ثبت، مشاهده و ارسال درخواست تدریس و زمان آزاد استاد»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [ProfessorTeachingRequestsController.cs](../src/UniversitySystem.Api/Controllers/ProfessorTeachingRequestsController.cs) |
| `StudentEnrollmentsController` | ورودی HTTP بخش «ثبت‌نام قطعی و مشاهده ثبت‌نام دانشجو»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [StudentEnrollmentsController.cs](../src/UniversitySystem.Api/Controllers/StudentEnrollmentsController.cs) |
| `StudentPreRegistrationController` | ورودی HTTP بخش «درس‌های مجاز، ذخیره و ارسال پیش‌انتخاب دانشجو»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [StudentPreRegistrationController.cs](../src/UniversitySystem.Api/Controllers/StudentPreRegistrationController.cs) |
| `StudentResultsController` | ورودی HTTP بخش «نتیجه پیش‌انتخاب دانشجو»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد. | [StudentResultsController.cs](../src/UniversitySystem.Api/Controllers/StudentResultsController.cs) |
| `TestAuthController` | مسیرهای موقت بررسی دسترسی نقش‌ها؛ فرایند آموزشی واقعی انجام نمی‌دهد. | [TestAuthController.cs](../src/UniversitySystem.Api/Controllers/TestAuthController.cs) |

## Api / UniversitySystem.Api

| کلاس | مسئولیت | فایل |
|---|---|---|
| `DependencyInjection` | سرویس‌های Persistence را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است. | [DependencyInjection.cs](../src/UniversitySystem.Api/DependencyInjection.cs) |
| `Program` | مدل کمکی Program؛ مسئولیت آن در راهنمای فارسی پروژه توضیح داده شده است. | [Program.cs](../src/UniversitySystem.Api/Program.cs) |

## Api / Infrastructure

| کلاس | مسئولیت | فایل |
|---|---|---|
| `GlobalExceptionHandler` | خطاهای اعتبارسنجی، دسترسی و قواعد کسب‌وکار را به پاسخ HTTP استاندارد ProblemDetails تبدیل می‌کند. | [GlobalExceptionHandler.cs](../src/UniversitySystem.Api/Infrastructure/GlobalExceptionHandler.cs) |

## Api / Serialization

| کلاس | مسئولیت | فایل |
|---|---|---|
| `SaveCourseOfferingScheduleRequestConverter` | تبدیل JSON ورودی زمان‌بندی؛ هر دو قالب آرایه و شیء دارای slots را به قرارداد یکسان تبدیل می‌کند. | [SaveCourseOfferingScheduleRequestConverter.cs](../src/UniversitySystem.Api/Serialization/SaveCourseOfferingScheduleRequestConverter.cs) |

## Application / Behaviors

| کلاس | مسئولیت | فایل |
|---|---|---|
| `LoggingBehavior` | اجرای درخواست‌های Application را برای پیگیری در لاگ ثبت می‌کند. | [LoggingBehavior.cs](../src/UniversitySystem.Application/Common/Behaviors/LoggingBehavior.cs) |
| `PerformanceBehavior` | مدت اجرای درخواست Application را اندازه می‌گیرد و درخواست کند را در لاگ مشخص می‌کند. | [PerformanceBehavior.cs](../src/UniversitySystem.Application/Common/Behaviors/PerformanceBehavior.cs) |
| `ValidationBehavior` | قبل از اجرای Handler، اعتبارسنج‌های درخواست را اجرا می‌کند و ورودی نامعتبر را متوقف می‌کند. | [ValidationBehavior.cs](../src/UniversitySystem.Application/Common/Behaviors/ValidationBehavior.cs) |

## Application / Exceptions

| کلاس | مسئولیت | فایل |
|---|---|---|
| `BusinessException` | خطای نقض قانون آموزشی مانند ظرفیت پر یا ویرایش درخواست ارسال‌شده؛ در API به پاسخ ۴۰۰ تبدیل می‌شود. | [BusinessException.cs](../src/UniversitySystem.Application/Common/Exceptions/BusinessException.cs) |
| `ForbiddenAccessException` | خطای نداشتن اجازه انجام عملیات؛ در API به پاسخ ۴۰۳ تبدیل می‌شود. | [ForbiddenAccessException.cs](../src/UniversitySystem.Application/Common/Exceptions/ForbiddenAccessException.cs) |
| `NotFoundException` | خطای پیدا نشدن اطلاعات درخواستی؛ در API به پاسخ ۴۰۴ تبدیل می‌شود. | [NotFoundException.cs](../src/UniversitySystem.Application/Common/Exceptions/NotFoundException.cs) |
| `ValidationException` | خطای ورودی نامعتبر همراه خطاهای هر فیلد؛ در API به پاسخ ۴۰۰ تبدیل می‌شود. | [ValidationException.cs](../src/UniversitySystem.Application/Common/Exceptions/ValidationException.cs) |

## Application / Interfaces

| کلاس | مسئولیت | فایل |
|---|---|---|
| `ICurrentUserService` | قرارداد دریافت شناسه و نقش کاربر جاری؛ مانع نیاز سرویس آموزشی به HttpContext می‌شود. | [ICurrentUserService.cs](../src/UniversitySystem.Application/Common/Interfaces/ICurrentUserService.cs) |
| `IDateTimeProvider` | قرارداد دریافت زمان؛ امکان استفاده از زمان کنترل‌شده در تست را فراهم می‌کند. | [IDateTimeProvider.cs](../src/UniversitySystem.Application/Common/Interfaces/IDateTimeProvider.cs) |
| `IPasswordHasher` | قرارداد هش و بررسی رمز؛ سرویس ورود را از روش فنی هش مستقل می‌کند. | [IPasswordHasher.cs](../src/UniversitySystem.Application/Common/Interfaces/IPasswordHasher.cs) |
| `IStudentCourseEligibilityService` | قرارداد محاسبه درس‌های مجاز دانشجو با توجه به چارت، درس‌های پاس‌شده و پیش‌نیازها؛ در پیش‌انتخاب و ثبت‌نام دوباره استفاده می‌شود. | [IStudentCourseEligibilityService.cs](../src/UniversitySystem.Application/Common/Interfaces/IStudentCourseEligibilityService.cs) |
| `ITokenService` | قرارداد تولید توکن ورود و زمان انقضا؛ تنظیمات امضای JWT در Infrastructure قرار دارند. | [ITokenService.cs](../src/UniversitySystem.Application/Common/Interfaces/ITokenService.cs) |
| `IUnitOfWork` | قرارداد ذخیره تغییرات یک درخواست؛ سرویس Application را از پیاده‌سازی EF مستقل نگه می‌دارد. | [IUnitOfWork.cs](../src/UniversitySystem.Application/Common/Interfaces/IUnitOfWork.cs) |

## Application / Services

| کلاس | مسئولیت | فایل |
|---|---|---|
| `StudentCourseEligibilityService` | قوانین انتخاب درس دانشجو را اعمال می‌کند: درس پاس‌شده حذف می‌شود و همه پیش‌نیازها باید پاس شده باشند؛ داده از ریپازیتوری دریافت می‌شود. | [StudentCourseEligibilityService.cs](../src/UniversitySystem.Application/Common/Services/StudentCourseEligibilityService.cs) |

## Application / UniversitySystem.Application

| کلاس | مسئولیت | فایل |
|---|---|---|
| `DependencyInjection` | سرویس‌های Persistence را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است. | [DependencyInjection.cs](../src/UniversitySystem.Application/DependencyInjection.cs) |

## Application / AdminPlanning

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AcademicPlanningOverviewDto` | نمای برنامه‌ریزی یک ترم؛ مشخصات ترم و درس‌های دارای تقاضای دانشجو یا علاقه استاد را جمع می‌کند. | [AcademicPlanningOverviewDto.cs](../src/UniversitySystem.Application/Features/AdminPlanning/DTOs/AcademicPlanningOverviewDto.cs) |
| `CoursePlanningOverviewDto` | یک درس در نمای برنامه‌ریزی: واحد، تعداد تقاضای دانشجو و فهرست استادهای علاقه‌مند همراه اولویت. | [CoursePlanningOverviewDto.cs](../src/UniversitySystem.Application/Features/AdminPlanning/DTOs/CoursePlanningOverviewDto.cs) |
| `ProfessorInterestDto` | شناسه، نام و اولویت استاد علاقه‌مند به درس؛ از درخواست ارسال‌شده استخراج می‌شود. | [ProfessorInterestDto.cs](../src/UniversitySystem.Application/Features/AdminPlanning/DTOs/ProfessorInterestDto.cs) |
| `GetAcademicPlanningOverviewQuery` | درخواست خواندن اطلاعات برای «دریافت نمای برنامه‌ریزی آموزش»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetAcademicPlanningOverviewQuery.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Queries/GetAcademicPlanningOverview/GetAcademicPlanningOverviewQuery.cs) |
| `GetAcademicPlanningOverviewQueryHandler` | درخواست «دریافت نمای برنامه‌ریزی آموزش» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetAcademicPlanningOverviewQueryHandler.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Queries/GetAcademicPlanningOverview/GetAcademicPlanningOverviewQueryHandler.cs) |
| `GetAcademicPlanningOverviewQueryValidator` | اعتبارسنجی ورودی عملیات «دریافت نمای برنامه‌ریزی آموزش» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetAcademicPlanningOverviewQueryValidator.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Queries/GetAcademicPlanningOverview/GetAcademicPlanningOverviewQueryValidator.cs) |
| `AcademicTermInfoModel` | خروجی سبک ریپازیتوری برنامه‌ریزی شامل شناسه، کد و عنوان ترم؛ موجودیت دیتابیس جدید نیست. | [AcademicTermInfoModel.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Repositories/AcademicTermInfoModel.cs) |
| `CourseDemandCountModel` | خروجی تجمیع تقاضا شامل شناسه درس و تعداد درخواست‌های ارسال‌شده دانشجو. | [CourseDemandCountModel.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Repositories/CourseDemandCountModel.cs) |
| `CourseInfoModel` | خروجی سبک مشخصات درس برای گزارش برنامه‌ریزی؛ شامل شناسه، کد، عنوان و واحد است. | [CourseInfoModel.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Repositories/CourseInfoModel.cs) |
| `IAdminPlanningRepository` | قرارداد دسترسی به داده بخش «برنامه‌ریزی آموزش بر اساس تقاضا و علاقه استاد»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IAdminPlanningRepository.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Repositories/IAdminPlanningRepository.cs) |
| `ProfessorCourseInterestModel` | خروجی علاقه استاد به درس شامل شناسه‌ها، نام استاد و اولویت؛ برای جمع‌بندی آموزش استفاده می‌شود. | [ProfessorCourseInterestModel.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Repositories/ProfessorCourseInterestModel.cs) |
| `AdminPlanningService` | تقاضای پیش‌انتخاب ارسال‌شده و علاقه استادهای ارسال‌شده را برای هر درس کنار هم قرار می‌دهد و بر اساس تقاضا مرتب می‌کند؛ تصمیم ارائه را خودکار نمی‌گیرد. | [AdminPlanningService.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Services/AdminPlanningService.cs) |
| `IAdminPlanningService` | قرارداد عملیات بخش «برنامه‌ریزی آموزش بر اساس تقاضا و علاقه استاد»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد. | [IAdminPlanningService.cs](../src/UniversitySystem.Application/Features/AdminPlanning/Services/IAdminPlanningService.cs) |

## Application / AdminPreRegistration

| کلاس | مسئولیت | فایل |
|---|---|---|
| `CourseDemandDto` | خلاصه تقاضای یک درس از پیش‌انتخاب‌های ارسال‌شده دانشجویان؛ برای تصمیم ارائه استفاده می‌شود. | [CourseDemandDto.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/DTOs/CourseDemandDto.cs) |
| `GetCourseDemandSummaryQuery` | درخواست خواندن اطلاعات برای «دریافت تقاضای درس از درخواست‌های ارسال‌شده»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetCourseDemandSummaryQuery.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Queries/GetCourseDemandSummary/GetCourseDemandSummaryQuery.cs) |
| `GetCourseDemandSummaryQueryHandler` | درخواست «دریافت تقاضای درس از درخواست‌های ارسال‌شده» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetCourseDemandSummaryQueryHandler.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Queries/GetCourseDemandSummary/GetCourseDemandSummaryQueryHandler.cs) |
| `GetCourseDemandSummaryQueryValidator` | اعتبارسنجی ورودی عملیات «دریافت تقاضای درس از درخواست‌های ارسال‌شده» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetCourseDemandSummaryQueryValidator.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Queries/GetCourseDemandSummary/GetCourseDemandSummaryQueryValidator.cs) |
| `CourseDemandAggregatedModel` | مدل کمکی CourseDemandAggregatedModel؛ مسئولیت آن در راهنمای فارسی پروژه توضیح داده شده است. | [IAdminPreRegistrationRepository.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Repositories/IAdminPreRegistrationRepository.cs) |
| `IAdminPreRegistrationRepository` | قرارداد دسترسی به داده بخش «جمع‌بندی تقاضای درس دانشجویان»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IAdminPreRegistrationRepository.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Repositories/IAdminPreRegistrationRepository.cs) |
| `AdminPreRegistrationService` | اجرای قواعد و هماهنگی عملیات بخش «جمع‌بندی تقاضای درس دانشجویان»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد. | [AdminPreRegistrationService.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Services/AdminPreRegistrationService.cs) |
| `IAdminPreRegistrationService` | قرارداد عملیات بخش «جمع‌بندی تقاضای درس دانشجویان»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد. | [IAdminPreRegistrationService.cs](../src/UniversitySystem.Application/Features/AdminPreRegistration/Services/IAdminPreRegistrationService.cs) |

## Application / AdminReports

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AdminReportDto` | گزارش یک ترم: مجموع ظرفیت ارائه‌های فعال، مجموع ثبت‌نام فعال، ردیف ارائه‌ها و درس‌های دارای تقاضا بدون ارائه فعال. | [AdminReportDto.cs](../src/UniversitySystem.Application/Features/AdminReports/DTOs/AdminReportDto.cs) |
| `OfferingReportDto` | یک ردیف گزارش آموزش: درس و ارائه، ظرفیت، تعداد ثبت‌نام فعال، ظرفیت باقی‌مانده، تعداد استاد و بازه‌های کلاس. | [OfferingReportDto.cs](../src/UniversitySystem.Application/Features/AdminReports/DTOs/OfferingReportDto.cs) |
| `GetAdminReportHandler` | درخواست «دریافت گزارش ظرفیت و ثبت‌نام آموزش» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetAdminReportHandler.cs](../src/UniversitySystem.Application/Features/AdminReports/Queries/GetAdminReport/GetAdminReportHandler.cs) |
| `GetAdminReportQuery` | درخواست خواندن اطلاعات برای «دریافت گزارش ظرفیت و ثبت‌نام آموزش»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetAdminReportQuery.cs](../src/UniversitySystem.Application/Features/AdminReports/Queries/GetAdminReport/GetAdminReportQuery.cs) |
| `GetAdminReportValidator` | اعتبارسنجی ورودی عملیات «دریافت گزارش ظرفیت و ثبت‌نام آموزش» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetAdminReportValidator.cs](../src/UniversitySystem.Application/Features/AdminReports/Queries/GetAdminReport/GetAdminReportValidator.cs) |
| `IAdminReportRepository` | قرارداد دسترسی به داده بخش «گزارش ظرفیت و وضعیت ارائه‌های آموزش»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IAdminReportRepository.cs](../src/UniversitySystem.Application/Features/AdminReports/Repositories/IAdminReportRepository.cs) |
| `AdminReportService` | داده گزارش ارائه‌ها را با نمای تقاضای آموزش ترکیب می‌کند و درس‌های دارای تقاضا بدون ارائه فعال و مجموع ظرفیت و ثبت‌نام را برمی‌گرداند؛ داده را تغییر نمی‌دهد. | [AdminReportService.cs](../src/UniversitySystem.Application/Features/AdminReports/Services/AdminReportService.cs) |

## Application / Auth

| کلاس | مسئولیت | فایل |
|---|---|---|
| `LoginCommand` | درخواست انجام عملیات «ورود کاربر و دریافت توکن»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [LoginCommand.cs](../src/UniversitySystem.Application/Features/Auth/Commands/Login/LoginCommand.cs) |
| `LoginCommandHandler` | درخواست «ورود کاربر و دریافت توکن» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [LoginCommandHandler.cs](../src/UniversitySystem.Application/Features/Auth/Commands/Login/LoginCommandHandler.cs) |
| `LoginCommandValidator` | اعتبارسنجی ورودی عملیات «ورود کاربر و دریافت توکن» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [LoginCommandValidator.cs](../src/UniversitySystem.Application/Features/Auth/Commands/Login/LoginCommandValidator.cs) |
| `LoginResponse` | پاسخ ورود موفق شامل توکن دسترسی، زمان انقضا، شناسه و نام کاربر و نقش‌های او. | [LoginResponse.cs](../src/UniversitySystem.Application/Features/Auth/Commands/Login/LoginResponse.cs) |
| `IAuthRepository` | قرارداد دسترسی به داده بخش «ورود و احراز هویت»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IAuthRepository.cs](../src/UniversitySystem.Application/Features/Auth/Repositories/IAuthRepository.cs) |
| `AuthService` | حساب فعال کاربر را پیدا می‌کند، رمز را بررسی می‌کند و با نقش‌های کاربر توکن ورود می‌سازد. | [AuthService.cs](../src/UniversitySystem.Application/Features/Auth/Services/AuthService.cs) |
| `IAuthService` | قرارداد عملیات بخش «ورود و احراز هویت»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد. | [IAuthService.cs](../src/UniversitySystem.Application/Features/Auth/Services/IAuthService.cs) |

## Application / CourseOfferings

| کلاس | مسئولیت | فایل |
|---|---|---|
| `CreateCourseOfferingCommand` | درخواست انجام عملیات «ایجاد ارائه درس با ظرفیت مشخص»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [CreateCourseOfferingCommand.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/CreateCourseOffering/CreateCourseOfferingCommand.cs) |
| `CreateCourseOfferingCommandHandler` | درخواست «ایجاد ارائه درس با ظرفیت مشخص» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [CreateCourseOfferingCommandHandler.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/CreateCourseOffering/CreateCourseOfferingCommandHandler.cs) |
| `CreateCourseOfferingCommandValidator` | اعتبارسنجی ورودی عملیات «ایجاد ارائه درس با ظرفیت مشخص» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [CreateCourseOfferingCommandValidator.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/CreateCourseOffering/CreateCourseOfferingCommandValidator.cs) |
| `SaveCourseOfferingScheduleCommand` | درخواست انجام عملیات «ذخیره زمان‌بندی ارائه با کنترل تداخل»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [SaveCourseOfferingScheduleCommand.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/SaveCourseOfferingSchedule/SaveCourseOfferingScheduleCommand.cs) |
| `SaveCourseOfferingScheduleCommandHandler` | درخواست «ذخیره زمان‌بندی ارائه با کنترل تداخل» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [SaveCourseOfferingScheduleCommandHandler.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/SaveCourseOfferingSchedule/SaveCourseOfferingScheduleCommandHandler.cs) |
| `SaveCourseOfferingScheduleCommandValidator` | اعتبارسنجی ورودی عملیات «ذخیره زمان‌بندی ارائه با کنترل تداخل» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [SaveCourseOfferingScheduleCommandValidator.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/SaveCourseOfferingSchedule/SaveCourseOfferingScheduleCommandValidator.cs) |
| `UpdateCourseOfferingCommand` | درخواست انجام عملیات «ویرایش ظرفیت و وضعیت ارائه»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [UpdateCourseOfferingCommand.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/UpdateCourseOffering/UpdateCourseOfferingCommand.cs) |
| `UpdateCourseOfferingCommandHandler` | درخواست «ویرایش ظرفیت و وضعیت ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [UpdateCourseOfferingCommandHandler.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/UpdateCourseOffering/UpdateCourseOfferingCommandHandler.cs) |
| `UpdateCourseOfferingCommandValidator` | اعتبارسنجی ورودی عملیات «ویرایش ظرفیت و وضعیت ارائه» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [UpdateCourseOfferingCommandValidator.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Commands/UpdateCourseOffering/UpdateCourseOfferingCommandValidator.cs) |
| `CourseOfferingDto` | پاسخ مشخصات ارائه: شناسه ارائه، ترم، مشخصات درس، ظرفیت و وضعیت فعالیت. | [CourseOfferingDto.cs](../src/UniversitySystem.Application/Features/CourseOfferings/DTOs/CourseOfferingDto.cs) |
| `CourseOfferingScheduleDto` | پاسخ یک بازه ثبت‌شده کلاس شامل شناسه بازه و ارائه، روز هفته و زمان شروع و پایان. | [CourseOfferingScheduleDto.cs](../src/UniversitySystem.Application/Features/CourseOfferings/DTOs/CourseOfferingScheduleDto.cs) |
| `CourseOfferingScheduleSlotDto` | ورودی روز و زمان کلاس برای ذخیره برنامه؛ با زمان آزاد پیشنهادی استاد متفاوت است. | [CourseOfferingScheduleSlotDto.cs](../src/UniversitySystem.Application/Features/CourseOfferings/DTOs/CourseOfferingScheduleSlotDto.cs) |
| `GetCourseOfferingsQuery` | درخواست خواندن اطلاعات برای «دریافت ارائه‌های یک ترم»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetCourseOfferingsQuery.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Queries/GetCourseOfferings/GetCourseOfferingsQuery.cs) |
| `GetCourseOfferingsQueryHandler` | درخواست «دریافت ارائه‌های یک ترم» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetCourseOfferingsQueryHandler.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Queries/GetCourseOfferings/GetCourseOfferingsQueryHandler.cs) |
| `GetCourseOfferingsQueryValidator` | اعتبارسنجی ورودی عملیات «دریافت ارائه‌های یک ترم» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetCourseOfferingsQueryValidator.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Queries/GetCourseOfferings/GetCourseOfferingsQueryValidator.cs) |
| `GetCourseOfferingScheduleQuery` | درخواست خواندن اطلاعات برای «مشاهده زمان‌بندی ارائه»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetCourseOfferingScheduleQuery.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Queries/GetCourseOfferingSchedule/GetCourseOfferingScheduleQuery.cs) |
| `GetCourseOfferingScheduleQueryHandler` | درخواست «مشاهده زمان‌بندی ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetCourseOfferingScheduleQueryHandler.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Queries/GetCourseOfferingSchedule/GetCourseOfferingScheduleQueryHandler.cs) |
| `ICourseOfferingRepository` | قرارداد دسترسی به داده بخش «ارائه درس و برنامه زمانی کلاس»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [ICourseOfferingRepository.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Repositories/ICourseOfferingRepository.cs) |
| `IProfessorScheduleRepository` | قرارداد دسترسی به داده بخش «ارائه درس و برنامه زمانی کلاس»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IProfessorScheduleRepository.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Repositories/IProfessorScheduleRepository.cs) |
| `ProfessorScheduleData` | داده موردنیاز تداخل‌سنجی یک استاد: نام، برنامه ارائه‌های دیگر همان ترم و زمان‌های آزاد اعلام‌شده. | [ProfessorScheduleData.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Repositories/ProfessorScheduleData.cs) |
| `CourseOfferingService` | اجرای قواعد و هماهنگی عملیات بخش «ارائه درس و برنامه زمانی کلاس»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد. | [CourseOfferingService.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Services/CourseOfferingService.cs) |
| `ICourseOfferingService` | قرارداد عملیات بخش «ارائه درس و برنامه زمانی کلاس»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد. | [ICourseOfferingService.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Services/ICourseOfferingService.cs) |
| `IProfessorScheduleConflictChecker` | مدل کمکی IProfessorScheduleConflictChecker؛ مسئولیت آن در راهنمای فارسی پروژه توضیح داده شده است. | [IProfessorScheduleConflictChecker.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Services/IProfessorScheduleConflictChecker.cs) |
| `ProfessorScheduleConflictChecker` | قانون مشترک بررسی تداخل استاد و پوشش زمان کلاس توسط زمان‌های آزاد؛ هنگام ذخیره برنامه و تخصیص استاد استفاده می‌شود. | [ProfessorScheduleConflictChecker.cs](../src/UniversitySystem.Application/Features/CourseOfferings/Services/ProfessorScheduleConflictChecker.cs) |

## Application / Enrollments

| کلاس | مسئولیت | فایل |
|---|---|---|
| `CreateEnrollmentCommand` | درخواست انجام عملیات «ثبت‌نام قطعی دانشجو در ارائه»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [CreateEnrollmentCommand.cs](../src/UniversitySystem.Application/Features/Enrollments/Commands/CreateEnrollment/CreateEnrollmentCommand.cs) |
| `CreateEnrollmentHandler` | درخواست «ثبت‌نام قطعی دانشجو در ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [CreateEnrollmentHandler.cs](../src/UniversitySystem.Application/Features/Enrollments/Commands/CreateEnrollment/CreateEnrollmentHandler.cs) |
| `CreateEnrollmentValidator` | اعتبارسنجی ورودی عملیات «ثبت‌نام قطعی دانشجو در ارائه» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [CreateEnrollmentValidator.cs](../src/UniversitySystem.Application/Features/Enrollments/Commands/CreateEnrollment/CreateEnrollmentValidator.cs) |
| `EnrollmentDto` | پاسخ ثبت‌نام قطعی شامل شناسه ثبت‌نام و ارائه، ترم، مشخصات درس، وضعیت و زمان ثبت‌نام. | [EnrollmentDto.cs](../src/UniversitySystem.Application/Features/Enrollments/DTOs/EnrollmentDto.cs) |
| `GetStudentEnrollmentsHandler` | درخواست «مشاهده ثبت‌نام‌های دانشجو در ترم» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetStudentEnrollmentsHandler.cs](../src/UniversitySystem.Application/Features/Enrollments/Queries/GetStudentEnrollments/GetStudentEnrollmentsHandler.cs) |
| `GetStudentEnrollmentsQuery` | درخواست خواندن اطلاعات برای «مشاهده ثبت‌نام‌های دانشجو در ترم»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetStudentEnrollmentsQuery.cs](../src/UniversitySystem.Application/Features/Enrollments/Queries/GetStudentEnrollments/GetStudentEnrollmentsQuery.cs) |
| `GetStudentEnrollmentsValidator` | اعتبارسنجی ورودی عملیات «مشاهده ثبت‌نام‌های دانشجو در ترم» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetStudentEnrollmentsValidator.cs](../src/UniversitySystem.Application/Features/Enrollments/Queries/GetStudentEnrollments/GetStudentEnrollmentsValidator.cs) |
| `IEnrollmentRepository` | قرارداد دسترسی به داده بخش «ثبت‌نام قطعی دانشجو»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IEnrollmentRepository.cs](../src/UniversitySystem.Application/Features/Enrollments/Repositories/IEnrollmentRepository.cs) |
| `EnrollmentService` | ثبت‌نام قطعی و مشاهده ثبت‌نام دانشجو را هماهنگ می‌کند؛ داخل تراکنش، شرایط آموزشی، ظرفیت، تکرار و تداخل را کنترل می‌کند و سپس Enrollment می‌سازد. | [EnrollmentService.cs](../src/UniversitySystem.Application/Features/Enrollments/Services/EnrollmentService.cs) |

## Application / ProfessorTeachingRequests

| کلاس | مسئولیت | فایل |
|---|---|---|
| `SaveAvailabilityCommand` | درخواست انجام عملیات «ثبت زمان‌های آزاد استاد»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [SaveAvailabilityCommand.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SaveAvailability/SaveAvailabilityCommand.cs) |
| `SaveAvailabilityCommandHandler` | درخواست «ثبت زمان‌های آزاد استاد» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [SaveAvailabilityCommandHandler.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SaveAvailability/SaveAvailabilityCommandHandler.cs) |
| `SaveAvailabilityValidator` | اعتبارسنجی ورودی عملیات «ثبت زمان‌های آزاد استاد» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [SaveAvailabilityValidator.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SaveAvailability/SaveAvailabilityValidator.cs) |
| `SaveTeachingRequestCommand` | درخواست انجام عملیات «ذخیره یا ویرایش درس‌های درخواست تدریس»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [SaveTeachingRequestCommand.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SaveTeachingRequest/SaveTeachingRequestCommand.cs) |
| `SaveTeachingRequestCommandHandler` | درخواست «ذخیره یا ویرایش درس‌های درخواست تدریس» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [SaveTeachingRequestCommandHandler.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SaveTeachingRequest/SaveTeachingRequestCommandHandler.cs) |
| `SaveTeachingRequestValidator` | اعتبارسنجی ورودی عملیات «ذخیره یا ویرایش درس‌های درخواست تدریس» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [SaveTeachingRequestValidator.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SaveTeachingRequest/SaveTeachingRequestValidator.cs) |
| `SubmitTeachingRequestCommand` | درخواست انجام عملیات «ارسال نهایی درخواست تدریس»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [SubmitTeachingRequestCommand.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SubmitTeachingRequest/SubmitTeachingRequestCommand.cs) |
| `SubmitTeachingRequestCommandHandler` | درخواست «ارسال نهایی درخواست تدریس» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [SubmitTeachingRequestCommandHandler.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SubmitTeachingRequest/SubmitTeachingRequestCommandHandler.cs) |
| `SubmitTeachingRequestValidator` | اعتبارسنجی ورودی عملیات «ارسال نهایی درخواست تدریس» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [SubmitTeachingRequestValidator.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Commands/SubmitTeachingRequest/SubmitTeachingRequestValidator.cs) |
| `AvailabilityInput` | ورودی و خروجی زمان آزاد استاد شامل روز هفته، ساعت شروع و پایان؛ زیرمجموعه درخواست تدریس است. | [AvailabilityInput.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/DTOs/AvailabilityInput.cs) |
| `TeachingCourseDto` | مشخصات نمایشی درس پیشنهادی استاد: شناسه، کد، عنوان، تعداد واحد و اولویت تدریس. | [TeachingCourseDto.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/DTOs/TeachingCourseDto.cs) |
| `TeachingCourseInput` | ورودی انتخاب درس استاد شامل شناسه درس و اولویت تدریس؛ تخصیص نهایی استاد به کلاس نیست. | [TeachingCourseInput.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/DTOs/TeachingCourseInput.cs) |
| `TeachingRequestDto` | پاسخ درخواست تدریس: شناسه درخواست، ترم، وضعیت، زمان ارسال، درس‌ها و بازه‌های آزاد استاد. | [TeachingRequestDto.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/DTOs/TeachingRequestDto.cs) |
| `TeachingRequestSummaryDto` | خلاصه مخصوص آموزش؛ شناسه و نام استاد را همراه جزئیات درخواست ارسال‌شده برمی‌گرداند. | [TeachingRequestSummaryDto.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/DTOs/TeachingRequestSummaryDto.cs) |
| `GetTeachingRequestQuery` | درخواست خواندن اطلاعات برای «مشاهده درخواست تدریس استاد جاری»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetTeachingRequestQuery.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Queries/GetTeachingRequest/GetTeachingRequestQuery.cs) |
| `GetTeachingRequestQueryHandler` | درخواست «مشاهده درخواست تدریس استاد جاری» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetTeachingRequestQueryHandler.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Queries/GetTeachingRequest/GetTeachingRequestQueryHandler.cs) |
| `GetTeachingRequestValidator` | اعتبارسنجی ورودی عملیات «مشاهده درخواست تدریس استاد جاری» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetTeachingRequestValidator.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Queries/GetTeachingRequest/GetTeachingRequestValidator.cs) |
| `GetTeachingRequestSummaryQuery` | درخواست خواندن اطلاعات برای «جمع‌بندی درخواست‌های ارسال‌شده استادها»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetTeachingRequestSummaryQuery.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Queries/GetTeachingRequestSummary/GetTeachingRequestSummaryQuery.cs) |
| `GetTeachingRequestSummaryQueryHandler` | درخواست «جمع‌بندی درخواست‌های ارسال‌شده استادها» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetTeachingRequestSummaryQueryHandler.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Queries/GetTeachingRequestSummary/GetTeachingRequestSummaryQueryHandler.cs) |
| `GetTeachingRequestSummaryValidator` | اعتبارسنجی ورودی عملیات «جمع‌بندی درخواست‌های ارسال‌شده استادها» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetTeachingRequestSummaryValidator.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Queries/GetTeachingRequestSummary/GetTeachingRequestSummaryValidator.cs) |
| `IProfessorTeachingRequestRepository` | قرارداد دسترسی به داده بخش «درخواست تدریس و زمان‌های آزاد استاد»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IProfessorTeachingRequestRepository.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Repositories/IProfessorTeachingRequestRepository.cs) |
| `ProfessorTeachingRequestService` | درخواست استاد جاری را مدیریت می‌کند: ذخیره درس‌ها، ویرایش زمان آزاد، ارسال نهایی و تهیه خلاصه درخواست‌های ارسال‌شده برای آموزش. | [ProfessorTeachingRequestService.cs](../src/UniversitySystem.Application/Features/ProfessorTeachingRequests/Services/ProfessorTeachingRequestService.cs) |

## Application / StudentPreRegistration

| کلاس | مسئولیت | فایل |
|---|---|---|
| `SaveStudentPreRegistrationCommand` | درخواست انجام عملیات «ذخیره یا ویرایش پیش‌نویس پیش‌انتخاب»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [SaveStudentPreRegistrationCommand.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Commands/SaveStudentPreRegistration/SaveStudentPreRegistrationCommand.cs) |
| `SaveStudentPreRegistrationCommandHandler` | درخواست «ذخیره یا ویرایش پیش‌نویس پیش‌انتخاب» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [SaveStudentPreRegistrationCommandHandler.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Commands/SaveStudentPreRegistration/SaveStudentPreRegistrationCommandHandler.cs) |
| `SaveStudentPreRegistrationCommandValidator` | اعتبارسنجی ورودی عملیات «ذخیره یا ویرایش پیش‌نویس پیش‌انتخاب» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [SaveStudentPreRegistrationCommandValidator.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Commands/SaveStudentPreRegistration/SaveStudentPreRegistrationCommandValidator.cs) |
| `SubmitStudentPreRegistrationCommand` | درخواست انجام عملیات «ارسال نهایی پیش‌انتخاب»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [SubmitStudentPreRegistrationCommand.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Commands/SubmitStudentPreRegistration/SubmitStudentPreRegistrationCommand.cs) |
| `SubmitStudentPreRegistrationCommandHandler` | درخواست «ارسال نهایی پیش‌انتخاب» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [SubmitStudentPreRegistrationCommandHandler.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Commands/SubmitStudentPreRegistration/SubmitStudentPreRegistrationCommandHandler.cs) |
| `SubmitStudentPreRegistrationCommandValidator` | اعتبارسنجی ورودی عملیات «ارسال نهایی پیش‌انتخاب» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [SubmitStudentPreRegistrationCommandValidator.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Commands/SubmitStudentPreRegistration/SubmitStudentPreRegistrationCommandValidator.cs) |
| `CoursePrerequisiteDto` | مشخصات نمایشی یک پیش‌نیاز: شناسه، کد و عنوان درس پیش‌نیاز؛ رابطه اصلی در CoursePrerequisite ذخیره می‌شود. | [CoursePrerequisiteDto.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/DTOs/CoursePrerequisiteDto.cs) |
| `EligibleCourseDto` | خروجی یک درس مجاز برای پیش‌انتخاب: کد، عنوان، تعداد واحد، ترم پیشنهادی، الزامی بودن و مشخصات پیش‌نیازها. | [EligibleCourseDto.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/DTOs/EligibleCourseDto.cs) |
| `PreRegistrationCourseItemDto` | مشخصات یک درس انتخاب‌شده در پاسخ پیش‌انتخاب: شناسه، کد، عنوان، واحد و اولویت دانشجو. | [PreRegistrationCourseItemDto.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/DTOs/PreRegistrationCourseItemDto.cs) |
| `SelectedCourseItemDto` | ورودی انتخاب درس دانشجو؛ فقط شناسه درس و اولویت را دریافت می‌کند و مالک درخواست از کاربر جاری تعیین می‌شود. | [SelectedCourseItemDto.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/DTOs/SelectedCourseItemDto.cs) |
| `StudentPreRegistrationDto` | پاسخ پیش‌انتخاب دانشجو: شناسه درخواست، ترم، وضعیت، درس‌های اولویت‌بندی‌شده، مجموع واحد و زمان ارسال. | [StudentPreRegistrationDto.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/DTOs/StudentPreRegistrationDto.cs) |
| `GetEligibleCoursesQuery` | درخواست خواندن اطلاعات برای «دریافت درس‌های مجاز دانشجو»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetEligibleCoursesQuery.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Queries/GetEligibleCourses/GetEligibleCoursesQuery.cs) |
| `GetEligibleCoursesQueryHandler` | درخواست «دریافت درس‌های مجاز دانشجو» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetEligibleCoursesQueryHandler.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Queries/GetEligibleCourses/GetEligibleCoursesQueryHandler.cs) |
| `GetEligibleCoursesQueryValidator` | اعتبارسنجی ورودی عملیات «دریافت درس‌های مجاز دانشجو» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetEligibleCoursesQueryValidator.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Queries/GetEligibleCourses/GetEligibleCoursesQueryValidator.cs) |
| `GetStudentPreRegistrationQuery` | درخواست خواندن اطلاعات برای «مشاهده پیش‌انتخاب دانشجو»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetStudentPreRegistrationQuery.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Queries/GetStudentPreRegistration/GetStudentPreRegistrationQuery.cs) |
| `GetStudentPreRegistrationQueryHandler` | درخواست «مشاهده پیش‌انتخاب دانشجو» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetStudentPreRegistrationQueryHandler.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Queries/GetStudentPreRegistration/GetStudentPreRegistrationQueryHandler.cs) |
| `EligibilityData` | داده موردنیاز بررسی مجاز بودن درس: درس‌های چارت با پیش‌نیازها و مجموعه شناسه درس‌های پاس‌شده. | [EligibilityData.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Repositories/EligibilityData.cs) |
| `IStudentEligibilityRepository` | قرارداد دسترسی به داده بخش «پیش‌انتخاب واحد دانشجو»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IStudentEligibilityRepository.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Repositories/IStudentEligibilityRepository.cs) |
| `IStudentPreRegistrationRepository` | قرارداد دسترسی به داده بخش «پیش‌انتخاب واحد دانشجو»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [IStudentPreRegistrationRepository.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Repositories/IStudentPreRegistrationRepository.cs) |
| `IStudentPreRegistrationService` | قرارداد عملیات بخش «پیش‌انتخاب واحد دانشجو»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد. | [IStudentPreRegistrationService.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Services/IStudentPreRegistrationService.cs) |
| `StudentPreRegistrationService` | اجرای قواعد و هماهنگی عملیات بخش «پیش‌انتخاب واحد دانشجو»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد. | [StudentPreRegistrationService.cs](../src/UniversitySystem.Application/Features/StudentPreRegistration/Services/StudentPreRegistrationService.cs) |

## Application / StudentResults

| کلاس | مسئولیت | فایل |
|---|---|---|
| `CourseResultDto` | نتیجه یک درس درخواستی دانشجو: مشخصات درس، اولویت، داشتن ارائه فعال و جزئیات ارائه‌ها. | [CourseResultDto.cs](../src/UniversitySystem.Application/Features/StudentResults/DTOs/CourseResultDto.cs) |
| `OfferingResultDto` | وضعیت یک ارائه برای دانشجو: ظرفیت کل و باقی‌مانده، فعالیت، استادهای تخصیص‌یافته و برنامه کلاس. | [OfferingResultDto.cs](../src/UniversitySystem.Application/Features/StudentResults/DTOs/OfferingResultDto.cs) |
| `ProfessorResultDto` | شناسه و نام استاد تخصیص‌یافته به ارائه درس در نتیجه پیش‌انتخاب دانشجو. | [ProfessorResultDto.cs](../src/UniversitySystem.Application/Features/StudentResults/DTOs/ProfessorResultDto.cs) |
| `ScheduleResultDto` | روز و بازه برگزاری کلاس در نتیجه پیش‌انتخاب؛ اطلاعات برنامه واقعی ارائه را نمایش می‌دهد. | [ScheduleResultDto.cs](../src/UniversitySystem.Application/Features/StudentResults/DTOs/ScheduleResultDto.cs) |
| `StudentResultDto` | پاسخ نتیجه پیش‌انتخاب؛ ترم، وضعیت درخواست و نتیجه ارائه هر درس درخواستی را جمع می‌کند. | [StudentResultDto.cs](../src/UniversitySystem.Application/Features/StudentResults/DTOs/StudentResultDto.cs) |
| `GetStudentResultHandler` | درخواست «دریافت نتیجه ارائه درس‌های پیش‌انتخاب» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetStudentResultHandler.cs](../src/UniversitySystem.Application/Features/StudentResults/Queries/GetStudentResult/GetStudentResultHandler.cs) |
| `GetStudentResultQuery` | درخواست خواندن اطلاعات برای «دریافت نتیجه ارائه درس‌های پیش‌انتخاب»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetStudentResultQuery.cs](../src/UniversitySystem.Application/Features/StudentResults/Queries/GetStudentResult/GetStudentResultQuery.cs) |
| `GetStudentResultValidator` | اعتبارسنجی ورودی عملیات «دریافت نتیجه ارائه درس‌های پیش‌انتخاب» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetStudentResultValidator.cs](../src/UniversitySystem.Application/Features/StudentResults/Queries/GetStudentResult/GetStudentResultValidator.cs) |
| `StudentResultService` | برای درس‌های پیش‌انتخاب ارسال‌شده دانشجو، ارائه‌ها، استادها، برنامه و ظرفیت باقی‌مانده را جمع می‌کند؛ پیش‌انتخاب را به ثبت‌نام تبدیل نمی‌کند. | [StudentResultService.cs](../src/UniversitySystem.Application/Features/StudentResults/Services/StudentResultService.cs) |

## Application / TeachingAssignments

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AssignProfessorCommand` | درخواست انجام عملیات «تخصیص استاد به ارائه»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [AssignProfessorCommand.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Commands/AssignProfessor/AssignProfessorCommand.cs) |
| `AssignProfessorCommandHandler` | درخواست «تخصیص استاد به ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [AssignProfessorCommandHandler.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Commands/AssignProfessor/AssignProfessorCommandHandler.cs) |
| `AssignProfessorCommandValidator` | اعتبارسنجی ورودی عملیات «تخصیص استاد به ارائه» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [AssignProfessorCommandValidator.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Commands/AssignProfessor/AssignProfessorCommandValidator.cs) |
| `RemoveProfessorAssignmentCommand` | درخواست انجام عملیات «حذف تخصیص استاد»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود. | [RemoveProfessorAssignmentCommand.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Commands/RemoveProfessorAssignment/RemoveProfessorAssignmentCommand.cs) |
| `RemoveProfessorAssignmentCommandHandler` | درخواست «حذف تخصیص استاد» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [RemoveProfessorAssignmentCommandHandler.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Commands/RemoveProfessorAssignment/RemoveProfessorAssignmentCommandHandler.cs) |
| `RemoveProfessorAssignmentCommandValidator` | اعتبارسنجی ورودی عملیات «حذف تخصیص استاد» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [RemoveProfessorAssignmentCommandValidator.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Commands/RemoveProfessorAssignment/RemoveProfessorAssignmentCommandValidator.cs) |
| `TeachingAssignmentDto` | پاسخ تخصیص استاد: شناسه تخصیص، نام استاد، زمان تخصیص و اینکه استاد قبلاً این درس را درخواست کرده است یا خیر. | [TeachingAssignmentDto.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/DTOs/TeachingAssignmentDto.cs) |
| `GetTeachingAssignmentsQuery` | درخواست خواندن اطلاعات برای «مشاهده استادهای تخصیص‌یافته»؛ هدف آن دریافت پاسخ بدون تغییر داده است. | [GetTeachingAssignmentsQuery.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Queries/GetTeachingAssignments/GetTeachingAssignmentsQuery.cs) |
| `GetTeachingAssignmentsQueryHandler` | درخواست «مشاهده استادهای تخصیص‌یافته» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند. | [GetTeachingAssignmentsQueryHandler.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Queries/GetTeachingAssignments/GetTeachingAssignmentsQueryHandler.cs) |
| `GetTeachingAssignmentsQueryValidator` | اعتبارسنجی ورودی عملیات «مشاهده استادهای تخصیص‌یافته» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند. | [GetTeachingAssignmentsQueryValidator.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Queries/GetTeachingAssignments/GetTeachingAssignmentsQueryValidator.cs) |
| `ITeachingAssignmentRepository` | قرارداد دسترسی به داده بخش «تخصیص استاد به ارائه درس»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند. | [ITeachingAssignmentRepository.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Repositories/ITeachingAssignmentRepository.cs) |
| `ITeachingAssignmentService` | قرارداد عملیات بخش «تخصیص استاد به ارائه درس»؛ پیاده‌سازی در سرویس هم‌نام قرار دارد. | [ITeachingAssignmentService.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Services/ITeachingAssignmentService.cs) |
| `TeachingAssignmentService` | اجرای قواعد و هماهنگی عملیات بخش «تخصیص استاد به ارائه درس»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد. | [TeachingAssignmentService.cs](../src/UniversitySystem.Application/Features/TeachingAssignments/Services/TeachingAssignmentService.cs) |

## Domain / Common

| کلاس | مسئولیت | فایل |
|---|---|---|
| `BaseAuditableEntity` | اطلاعات مشترک ایجاد و آخرین ویرایش رکورد، شامل زمان و کاربر انجام‌دهنده. | [BaseAuditableEntity.cs](../src/UniversitySystem.Domain/Common/BaseAuditableEntity.cs) |
| `BaseEntity` | شناسه مشترک موجودیت‌های دامنه؛ هویت هر رکورد را مشخص می‌کند. | [BaseEntity.cs](../src/UniversitySystem.Domain/Common/BaseEntity.cs) |

## Domain / Constants

| کلاس | مسئولیت | فایل |
|---|---|---|
| `RoleNames` | نام ثابت نقش‌های دسترسی؛ برای یکسان بودن نام نقش در مجوز API و توکن استفاده می‌شود. | [RoleNames.cs](../src/UniversitySystem.Domain/Constants/RoleNames.cs) |

## Domain / Entities

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AcademicTerm` | نیم‌سال تحصیلی با کد، عنوان و تاریخ شروع و پایان؛ درخواست‌ها و ارائه‌ها برای آن ثبت می‌شوند. | [AcademicTerm.cs](../src/UniversitySystem.Domain/Entities/AcademicTerm.cs) |
| `Course` | تعریف پایه درس شامل کد، عنوان و تعداد واحد؛ مستقل از ترم، استاد و ظرفیت کلاس است. | [Course.cs](../src/UniversitySystem.Domain/Entities/Course.cs) |
| `CourseOffering` | ارائه واقعی یک درس در یک ترم، همراه ظرفیت و وضعیت فعالیت؛ تخصیص استاد و زمان کلاس به آن متصل می‌شوند. | [CourseOffering.cs](../src/UniversitySystem.Domain/Entities/CourseOffering.cs) |
| `CourseOfferingSchedule` | روز و بازه زمانی برگزاری یک ارائه درس؛ برای نمایش برنامه و بررسی تداخل استفاده می‌شود. | [CourseOfferingSchedule.cs](../src/UniversitySystem.Domain/Entities/CourseOfferingSchedule.cs) |
| `CoursePrerequisite` | رابط میان درس و پیش‌نیاز آن؛ برای بررسی مجاز بودن انتخاب درس استفاده می‌شود. | [CoursePrerequisite.cs](../src/UniversitySystem.Domain/Entities/CoursePrerequisite.cs) |
| `Curriculum` | چارت یک رشته با نسخه مشخص؛ درس‌های پیشنهادی و الزامی رشته را در خود جمع می‌کند. | [Curriculum.cs](../src/UniversitySystem.Domain/Entities/Curriculum.cs) |
| `CurriculumCourse` | عضویت یک درس در چارت؛ ترم پیشنهادی و الزامی بودن درس را نگه می‌دارد. | [CurriculumCourse.cs](../src/UniversitySystem.Domain/Entities/CurriculumCourse.cs) |
| `Department` | گروه آموزشی زیرمجموعه دانشکده؛ رشته‌های مرتبط را دسته‌بندی می‌کند. | [Department.cs](../src/UniversitySystem.Domain/Entities/Department.cs) |
| `Enrollment` | ثبت‌نام قطعی دانشجو در یک ارائه؛ زمان ثبت‌نام، وضعیت و نمره نهایی را نگه می‌دارد. | [Enrollment.cs](../src/UniversitySystem.Domain/Entities/Enrollment.cs) |
| `Faculty` | دانشکده؛ بالاترین سطح ساختار آموزشی این پروژه و محل گروه‌های آموزشی است. | [Faculty.cs](../src/UniversitySystem.Domain/Entities/Faculty.cs) |
| `Major` | رشته تحصیلی دانشجو؛ برای پیدا کردن چارت درسی مناسب استفاده می‌شود. | [Major.cs](../src/UniversitySystem.Domain/Entities/Major.cs) |
| `Professor` | پروفایل استاد؛ حساب کاربری را به کد استادی، درخواست‌های تدریس و تخصیص‌های تدریس متصل می‌کند. | [Professor.cs](../src/UniversitySystem.Domain/Entities/Professor.cs) |
| `ProfessorAvailability` | بازه زمانی آزاد استاد در یک روز هفته، متعلق به درخواست تدریس؛ با زمان‌بندی نهایی کلاس تفاوت دارد. | [ProfessorAvailability.cs](../src/UniversitySystem.Domain/Entities/ProfessorAvailability.cs) |
| `ProfessorTeachingRequest` | درخواست تدریس استاد برای یک ترم؛ درس‌ها، اولویت‌ها و زمان‌های آزاد را جمع می‌کند و پس از ارسال قابل ویرایش نیست. | [ProfessorTeachingRequest.cs](../src/UniversitySystem.Domain/Entities/ProfessorTeachingRequest.cs) |
| `ProfessorTeachingRequestCourse` | یک درس پیشنهادی استاد در درخواست تدریس؛ علاقه و اولویت استاد را ثبت می‌کند و به معنی تخصیص نهایی نیست. | [ProfessorTeachingRequestCourse.cs](../src/UniversitySystem.Domain/Entities/ProfessorTeachingRequestCourse.cs) |
| `Role` | نقش دسترسی مانند دانشجو، استاد یا آموزش؛ تعیین می‌کند کاربر اجازه استفاده از کدام API را دارد. | [Role.cs](../src/UniversitySystem.Domain/Entities/Role.cs) |
| `Student` | پروفایل دانشجو؛ حساب کاربری را به شماره دانشجویی، رشته و سال ورود متصل می‌کند. | [Student.cs](../src/UniversitySystem.Domain/Entities/Student.cs) |
| `StudentCourseHistory` | سابقه درس دانشجو شامل ترم، نمره و وضعیت قبولی؛ در بررسی درس‌های پاس‌شده و پیش‌نیازها استفاده می‌شود. | [StudentCourseHistory.cs](../src/UniversitySystem.Domain/Entities/StudentCourseHistory.cs) |
| `StudentPreRegistration` | درخواست پیش‌انتخاب دانشجو برای یک ترم؛ درس‌های موردنیاز و وضعیت پیش‌نویس یا ارسال‌شده را مدیریت می‌کند. | [StudentPreRegistration.cs](../src/UniversitySystem.Domain/Entities/StudentPreRegistration.cs) |
| `StudentPreRegistrationItem` | یک درس در درخواست پیش‌انتخاب؛ شناسه درس و اولویت دانشجو را نگه می‌دارد و ثبت‌نام قطعی نیست. | [StudentPreRegistrationItem.cs](../src/UniversitySystem.Domain/Entities/StudentPreRegistrationItem.cs) |
| `TeachingAssignment` | تخصیص نهایی یک استاد به ارائه درس، همراه زمان تخصیص؛ مستقل از اعلام علاقه استاد است. | [TeachingAssignment.cs](../src/UniversitySystem.Domain/Entities/TeachingAssignment.cs) |
| `User` | حساب ورود کاربر؛ نام کاربری، هش رمز و وضعیت فعال بودن را نگه می‌دارد و به نقش‌ها و پروفایل دانشجو یا استاد متصل است. | [User.cs](../src/UniversitySystem.Domain/Entities/User.cs) |
| `UserRole` | رابط میان حساب کاربر و نقش؛ یک کاربر می‌تواند چند نقش داشته باشد. | [UserRole.cs](../src/UniversitySystem.Domain/Entities/UserRole.cs) |

## Domain / Enums

| کلاس | مسئولیت | فایل |
|---|---|---|
| `CourseEnrollmentStatus` | وضعیت سابقه درسی: در جریان، قبول، مردود یا حذف‌شده. | [CourseEnrollmentStatus.cs](../src/UniversitySystem.Domain/Enums/CourseEnrollmentStatus.cs) |
| `EnrollmentStatus` | وضعیت ثبت‌نام قطعی: ثبت‌نام‌شده، گذرانده، مردود یا حذف‌شده. | [EnrollmentStatus.cs](../src/UniversitySystem.Domain/Enums/EnrollmentStatus.cs) |
| `RequestStatus` | وضعیت درخواست پیش‌انتخاب یا تدریس: پیش‌نویس، ارسال‌شده و لغوشده. | [RequestStatus.cs](../src/UniversitySystem.Domain/Enums/RequestStatus.cs) |

## Infrastructure / Authentication

| کلاس | مسئولیت | فایل |
|---|---|---|
| `JwtSettings` | تنظیمات توکن شامل صادرکننده، مخاطب، کلید امضا و مدت اعتبار؛ مدل آموزشی یا جدول دیتابیس نیست. | [JwtSettings.cs](../src/UniversitySystem.Infrastructure/Authentication/JwtSettings.cs) |
| `PasswordHasher` | رمز را هش می‌کند و رمز ورودی را با هش ذخیره‌شده بررسی می‌کند؛ رمز خام ذخیره نمی‌شود. | [PasswordHasher.cs](../src/UniversitySystem.Infrastructure/Authentication/PasswordHasher.cs) |
| `TokenService` | توکن JWT را با شناسه و نقش‌های کاربر و زمان انقضا تولید می‌کند. | [TokenService.cs](../src/UniversitySystem.Infrastructure/Authentication/TokenService.cs) |

## Infrastructure / UniversitySystem.Infrastructure

| کلاس | مسئولیت | فایل |
|---|---|---|
| `DependencyInjection` | سرویس‌های Persistence را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است. | [DependencyInjection.cs](../src/UniversitySystem.Infrastructure/DependencyInjection.cs) |

## Infrastructure / Services

| کلاس | مسئولیت | فایل |
|---|---|---|
| `CurrentUserService` | شناسه و نقش کاربر احراز هویت‌شده را از درخواست HTTP دریافت می‌کند؛ در حالت ناشناس خروجی امن و خالی دارد. | [CurrentUserService.cs](../src/UniversitySystem.Infrastructure/Services/CurrentUserService.cs) |
| `SystemDateTimeProvider` | زمان محلی و UTC سیستم را ارائه می‌دهد؛ ذخیره زمان‌های آموزشی و حسابرسی از UTC استفاده می‌کند. | [SystemDateTimeProvider.cs](../src/UniversitySystem.Infrastructure/Services/SystemDateTimeProvider.cs) |

## Persistence / Configurations

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AcademicTermConfiguration` | تنظیم نگاشت مدل AcademicTerm به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [AcademicTermConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/AcademicTermConfiguration.cs) |
| `CourseConfiguration` | تنظیم نگاشت مدل Course به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [CourseConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/CourseConfiguration.cs) |
| `CourseOfferingConfiguration` | تنظیم نگاشت مدل CourseOffering به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [CourseOfferingConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/CourseOfferingConfiguration.cs) |
| `CourseOfferingScheduleConfiguration` | تنظیم نگاشت مدل CourseOfferingSchedule به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [CourseOfferingScheduleConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/CourseOfferingScheduleConfiguration.cs) |
| `CoursePrerequisiteConfiguration` | تنظیم نگاشت مدل CoursePrerequisite به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [CoursePrerequisiteConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/CoursePrerequisiteConfiguration.cs) |
| `CurriculumConfiguration` | تنظیم نگاشت مدل Curriculum به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [CurriculumConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/CurriculumConfiguration.cs) |
| `CurriculumCourseConfiguration` | تنظیم نگاشت مدل CurriculumCourse به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [CurriculumCourseConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/CurriculumCourseConfiguration.cs) |
| `DepartmentConfiguration` | تنظیم نگاشت مدل Department به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [DepartmentConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/DepartmentConfiguration.cs) |
| `EnrollmentConfiguration` | تنظیم نگاشت مدل Enrollment به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [EnrollmentConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/EnrollmentConfiguration.cs) |
| `FacultyConfiguration` | تنظیم نگاشت مدل Faculty به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [FacultyConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/FacultyConfiguration.cs) |
| `MajorConfiguration` | تنظیم نگاشت مدل Major به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [MajorConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/MajorConfiguration.cs) |
| `ProfessorAvailabilityConfiguration` | تنظیم نگاشت مدل ProfessorAvailability به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [ProfessorAvailabilityConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/ProfessorAvailabilityConfiguration.cs) |
| `ProfessorConfiguration` | تنظیم نگاشت مدل Professor به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [ProfessorConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/ProfessorConfiguration.cs) |
| `ProfessorTeachingRequestConfiguration` | تنظیم نگاشت مدل ProfessorTeachingRequest به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [ProfessorTeachingRequestConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/ProfessorTeachingRequestConfiguration.cs) |
| `ProfessorTeachingRequestCourseConfiguration` | تنظیم نگاشت مدل ProfessorTeachingRequestCourse به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [ProfessorTeachingRequestCourseConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/ProfessorTeachingRequestCourseConfiguration.cs) |
| `RoleConfiguration` | تنظیم نگاشت مدل Role به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [RoleConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/RoleConfiguration.cs) |
| `StudentConfiguration` | تنظیم نگاشت مدل Student به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [StudentConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/StudentConfiguration.cs) |
| `StudentCourseHistoryConfiguration` | تنظیم نگاشت مدل StudentCourseHistory به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [StudentCourseHistoryConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/StudentCourseHistoryConfiguration.cs) |
| `StudentPreRegistrationConfiguration` | تنظیم نگاشت مدل StudentPreRegistration به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [StudentPreRegistrationConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/StudentPreRegistrationConfiguration.cs) |
| `StudentPreRegistrationItemConfiguration` | تنظیم نگاشت مدل StudentPreRegistrationItem به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [StudentPreRegistrationItemConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/StudentPreRegistrationItemConfiguration.cs) |
| `TeachingAssignmentConfiguration` | تنظیم نگاشت مدل TeachingAssignment به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [TeachingAssignmentConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/TeachingAssignmentConfiguration.cs) |
| `UserConfiguration` | تنظیم نگاشت مدل User به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [UserConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/UserConfiguration.cs) |
| `UserRoleConfiguration` | تنظیم نگاشت مدل UserRole به جدول دیتابیس، شامل ستون‌ها، رابطه‌ها و محدودیت‌ها؛ منطق فرایند در این فایل قرار ندارد. | [UserRoleConfiguration.cs](../src/UniversitySystem.Persistence/Configurations/UserRoleConfiguration.cs) |

## Persistence / Data

| کلاس | مسئولیت | فایل |
|---|---|---|
| `ApplicationDbContext` | مرکز EF برای خواندن و ذخیره موجودیت‌ها؛ تنظیمات جدول‌ها را بارگذاری می‌کند و زمان‌های ثبت و ویرایش را به UTC می‌نویسد. | [ApplicationDbContext.cs](../src/UniversitySystem.Persistence/Data/ApplicationDbContext.cs) |
| `IApplicationDbContext` | قرارداد داخلی Persistence برای مجموعه‌های دیتابیس و آماده‌سازی داده تست؛ Application از ریپازیتوری‌های اختصاصی استفاده می‌کند. | [IApplicationDbContext.cs](../src/UniversitySystem.Persistence/Data/IApplicationDbContext.cs) |

## Persistence / UniversitySystem.Persistence

| کلاس | مسئولیت | فایل |
|---|---|---|
| `DependencyInjection` | سرویس‌های Persistence را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است. | [DependencyInjection.cs](../src/UniversitySystem.Persistence/DependencyInjection.cs) |

## Persistence / Repositories

| کلاس | مسئولیت | فایل |
|---|---|---|
| `AdminPlanningRepository` | دسترسی EF به اطلاعات ترم، مشخصات درس، تعداد تقاضا و علاقه استاد برای برنامه‌ریزی؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [AdminPlanningRepository.cs](../src/UniversitySystem.Persistence/Repositories/AdminPlanningRepository.cs) |
| `AdminPreRegistrationRepository` | دسترسی EF به تقاضای تجمیع‌شده از درخواست‌های ارسال‌شده دانشجویان؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [AdminPreRegistrationRepository.cs](../src/UniversitySystem.Persistence/Repositories/AdminPreRegistrationRepository.cs) |
| `AdminReportRepository` | دسترسی EF به تجمیع ظرفیت، ثبت‌نام فعال و تعداد استاد و بازه کلاس برای گزارش آموزش؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [AdminReportRepository.cs](../src/UniversitySystem.Persistence/Repositories/AdminReportRepository.cs) |
| `AuthRepository` | دسترسی EF به حساب کاربر و نقش‌های او برای ورود؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [AuthRepository.cs](../src/UniversitySystem.Persistence/Repositories/AuthRepository.cs) |
| `CourseOfferingRepository` | دسترسی EF به مشخصات ارائه، درس، ترم و بازه‌های کلاس؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [CourseOfferingRepository.cs](../src/UniversitySystem.Persistence/Repositories/CourseOfferingRepository.cs) |
| `EnrollmentRepository` | دسترسی EF به ارائه با اطلاعات آموزشی و ثبت‌نام‌های دانشجو و تعداد ثبت‌نام فعال؛ مرز تراکنش ثبت‌نام نیز در این پیاده‌سازی است؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [EnrollmentRepository.cs](../src/UniversitySystem.Persistence/Repositories/EnrollmentRepository.cs) |
| `ProfessorScheduleRepository` | دسترسی EF به برنامه سایر ارائه‌های فعال همان ترم و زمان‌های آزاد استاد برای بررسی تداخل؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [ProfessorScheduleRepository.cs](../src/UniversitySystem.Persistence/Repositories/ProfessorScheduleRepository.cs) |
| `ProfessorTeachingRequestRepository` | دسترسی EF به پروفایل استاد، درخواست تدریس، درس‌ها و زمان‌های آزاد؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [ProfessorTeachingRequestRepository.cs](../src/UniversitySystem.Persistence/Repositories/ProfessorTeachingRequestRepository.cs) |
| `StudentEligibilityRepository` | داده چارت فعال، درس‌های فعال، سوابق قبولی و پیش‌نیازها را از دیتابیس دریافت می‌کند؛ تصمیم نهایی انتخاب با سرویس Application است. | [StudentEligibilityRepository.cs](../src/UniversitySystem.Persistence/Repositories/StudentEligibilityRepository.cs) |
| `StudentPreRegistrationRepository` | دسترسی EF به پروفایل دانشجو، ترم و درخواست پیش‌انتخاب با درس‌های آن؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [StudentPreRegistrationRepository.cs](../src/UniversitySystem.Persistence/Repositories/StudentPreRegistrationRepository.cs) |
| `TeachingAssignmentRepository` | دسترسی EF به تخصیص استاد، پروفایل استاد و اطلاعات علاقه قبلی او به درس؛ تصمیم آموزشی در سرویس Application انجام می‌شود. | [TeachingAssignmentRepository.cs](../src/UniversitySystem.Persistence/Repositories/TeachingAssignmentRepository.cs) |
| `UnitOfWork` | تغییرات ریپازیتوری‌های همان درخواست را با DbContext مشترک ذخیره می‌کند؛ منطق آموزشی ندارد. | [UnitOfWork.cs](../src/UniversitySystem.Persistence/Repositories/UnitOfWork.cs) |

## فایل‌های خارج از فهرست کلاس‌ها

- `Program.cs`: ساخت میزبان، ثبت لایه‌ها، ترتیب میان‌افزارها و اتصال مسیرها.
- `appsettings*.json`: تنظیمات اجرا؛ اتصال دیتابیس در این مرحله تغییر نکرده است.
- فایل‌های `.csproj` و `.sln`: وابستگی‌ها و ساختار بیلد.
- `tests/UniversitySystem.IntegrationTests/UniversityApiFactory.cs`: میزبان مستقل تست با SQLite موقت؛ تست‌ها از دیتابیس اصلی استفاده نمی‌کنند.
- `tests/UniversitySystem.UnitTests`: بررسی‌های کوچک اعتبارسنجی، خطاها و سرویس‌های هویت.
- `tests/UniversitySystem.IntegrationTests`: بررسی HTTP، نقش‌ها و بخش‌های آموزشی؛ اجرای کامل آن‌ها برای مرحله بعد باقی مانده است.
