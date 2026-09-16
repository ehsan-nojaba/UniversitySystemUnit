# قرارداد اتصال UI به بک‌اند

## شروع کار

فرایند اصلی سه پنل آماده اتصال است: دانشجو، استاد و آموزش. ورود از `POST /api/v1/auth/login` انجام می‌شود. توکن پاسخ را در درخواست‌های بعدی با هدر `Authorization: Bearer TOKEN` ارسال کنید. سپس `GET /api/v1/auth/me` اطلاعات حساب، نقش‌ها و پروفایل را برمی‌گرداند. رمز و هش رمز در این پاسخ وجود ندارند.

```json
{"username":"نام کاربری موجود","password":"رمز کاربر"}
```

پاسخ ورود دارای `accessToken`، `expiresAt`، `userId`، `fullName` و `roles` است. نقش‌ها همان `Student`، `Professor` و `EducationAdmin` هستند؛ در UI عنوان نقش آخر را «کارشناس آموزش» نمایش دهید. `me` شامل `id`، `username`، `fullName`، `roles`، `student` و `professor` است. پروفایل نامرتبط null برمی‌گردد. نقش‌های me از داده حساب خوانده می‌شوند؛ مجوز درخواست‌های فعلی بر اساس نقش‌های توکن است و در صورت تغییر نقش باید ورود دوباره انجام شود.

## فهرست‌های فرم

| مسیر GET | دسترسی | استفاده در UI |
|---|---|---|
| `/api/v1/lookups/academic-terms` | هر سه نقش | انتخاب ترم؛ شامل ترم‌های فعال و غیرفعال برای مشاهده تاریخچه |
| `/api/v1/lookups/courses` | استاد و آموزش | انتخاب درس فعال برای درخواست تدریس یا ایجاد ارائه |
| `/api/v1/lookups/professors` | آموزش | انتخاب استاد دارای حساب فعال برای تخصیص |
| `/api/v1/student/pre-registration/eligible-courses?academicTermId=ID` | دانشجو | انتخاب درس بر اساس چارت و پیش‌نیازهای همان دانشجو |

گزینه ترم شامل id، code، title، isActive، startDate و endDate است. گزینه درس شامل id، code، title و credits است. گزینه استاد شامل id، personnelCode و fullName است. برای عملیات تغییر، ترم غیرفعال را انتخاب‌پذیر نکنید؛ سرور هم قواعد فعالیت را کنترل می‌کند. فهرست عمومی درس جای فهرست درس مجاز دانشجو نیست.

## پنل دانشجو

۱. انتخاب ترم از فهرست ترم‌ها.
۲. دریافت درس مجاز، مشاهده پیش‌انتخاب موجود و ذخیره انتخاب‌ها در `PUT /api/v1/student/pre-registration/{academicTermId}`:

```json
{"courses":[{"courseId":1,"priority":1},{"courseId":2,"priority":2}]}
```

۳. ارسال با `POST /api/v1/student/pre-registration/{academicTermId}/submit` بدون بدنه. وضعیت‌های درخواست رشته‌های `Draft`، `Submitted` و `Cancelled` هستند. پس از ارسال، فرم ویرایش را ببندید.
۴. دریافت نتیجه با `GET /api/v1/student/pre-registration/{academicTermId}/result`. برای هر درس، isOffered و فهرست offerings با استاد، برنامه و remainingCapacity موجود است. نتیجه برای درخواست ارسال‌شده دریافت می‌شود؛ پیش‌نویس نتیجه نهایی ندارد.
۵. ثبت‌نام با `POST /api/v1/student/enrollments`:

```json
{"courseOfferingId":1}
```

۶. مشاهده ثبت‌نام‌ها با `GET /api/v1/student/enrollments?academicTermId=ID`. ثبت‌نام جدید پاسخ ۲۰۱ دارد. ظرفیت نمایش‌داده‌شده لحظه‌ای است؛ اگر هنگام ثبت ظرفیت پر شود، خطای سرور را نشان دهید و نتیجه را دوباره دریافت کنید. پیش‌انتخاب صندلی رزرو نمی‌کند.

## پنل استاد

دریافت و ذخیره درس‌ها با `GET / PUT /api/v1/professor/teaching-request/{academicTermId}` انجام می‌شود. بدنه ذخیره مانند courses در پیش‌انتخاب است. نبود درخواست هنگام GET پاسخ ۴۰۴ می‌دهد و UI می‌تواند فرم جدید نمایش دهد.

ثبت زمان آزاد با `PUT /api/v1/professor/teaching-request/{academicTermId}/availability`:

```json
{"availability":[{"dayOfWeek":6,"startTime":"08:00:00","endTime":"12:00:00"}]}
```

Availability از درخواست پیش‌نویس موجود استفاده می‌کند؛ ابتدا درس‌ها را ذخیره کنید. آرایه خالی زمان‌های آزاد قبلی را حذف می‌کند. بازه‌های هم‌پوشان پذیرفته نمی‌شوند. ارسال با `POST /api/v1/professor/teaching-request/{academicTermId}/submit` بدون بدنه انجام می‌شود. پس از ارسال، ویرایش درس و زمان آزاد را ببندید. ثبت زمان آزاد اختیاری است؛ حداقل یک درس برای ارسال ضروری است.

## پنل آموزش

نمای برنامه‌ریزی: `GET /api/v1/admin/planning/overview?academicTermId=ID`.
تقاضای دانشجو: `GET /api/v1/admin/pre-registration/demand?academicTermId=ID`.
درخواست‌های استاد: `GET /api/v1/admin/teaching-requests?academicTermId=ID`.

ایجاد ارائه با `POST /api/v1/admin/course-offerings`:

```json
{"academicTermId":1,"courseId":1,"capacity":40}
```

پاسخ ۲۰۱ مشخصات ارائه و courseOfferingId را برمی‌گرداند. مشاهده ارائه‌ها: `GET /api/v1/admin/course-offerings?academicTermId=ID`. ویرایش ارائه با `PUT /api/v1/admin/course-offerings/{id}`:

```json
{"capacity":40,"isActive":true}
```

تخصیص استاد با `POST /api/v1/admin/course-offerings/{courseOfferingId}/professors`:

```json
{"professorId":1}
```

مشاهده استادها با GET همان مسیر و حذف تخصیص با `DELETE /api/v1/admin/course-offerings/{courseOfferingId}/professors/{professorId}` انجام می‌شود. انتخاب استاد از فهرست عمومی استادها است؛ علاقه قبلی استاد شرط اجباری تخصیص نیست.

ذخیره برنامه با `PUT /api/v1/admin/course-offerings/{courseOfferingId}/schedule`:

```json
{"slots":[{"dayOfWeek":6,"startTime":"08:00:00","endTime":"10:00:00"}]}
```

GET همان مسیر برنامه را برمی‌گرداند. آرایه خالی slots برنامه قبلی را پاک می‌کند. تداخل استاد و انطباق با زمان آزاد اعلام‌شده در سرور بررسی می‌شود. گزارش با `GET /api/v1/admin/reports?academicTermId=ID`، ظرفیت، ثبت‌نام فعال، وضعیت ارائه‌ها و درس‌های دارای تقاضا بدون ارائه فعال را برمی‌گرداند.

## روز، ساعت و خطا

DayOfWeek عدد است: یکشنبه ۰، دوشنبه ۱، سه‌شنبه ۲، چهارشنبه ۳، پنجشنبه ۴، جمعه ۵ و شنبه ۶. ساعت از نوع TimeOnly با قالب `HH:mm:ss` است. زمان‌های ارسال و ثبت‌نام UTC هستند و UI می‌تواند آن‌ها را به زمان محلی تبدیل کند. Priority باید مثبت باشد؛ عدد کمتر اولویت بالاتر است.

خطای ورودی یا قانون آموزشی: ۴۰۰؛ ورود یا توکن نامعتبر: ۴۰۱؛ نقش نامجاز: ۴۰۳؛ اطلاعات پیدا‌نشده: ۴۰۴. بیشتر خطاها ProblemDetails با title، detail و status هستند. خطای اعتبارسنجی، errors با پیام هر فیلد دارد. برخی مسیرهای GET برای نبود درخواست، ۴۰۴ بدون بدنه می‌دهند؛ UI باید بر اساس status هم تصمیم بگیرد. خطاهای ۵۰۰ را با پیام عمومی نمایش دهید.

## اتصال مرورگر

CORS برای `http://localhost:5173` و `http://localhost:3000` آماده است. برای آدرس دیگر، بدون تغییر کانکشن دیتابیس، آدرس UI را از تنظیمات اجرا تعیین کنید:

```powershell
$env:Cors__AllowedOrigins__0 = "https://آدرس-ui"
```

آدرس Origin باید دقیق و بدون مسیر یا اسلش پایانی باشد. این API از هدر Bearer استفاده می‌کند و CORS با کوکی فعال نشده است. API را با آدرس HTTPS تنظیم‌شده در launchSettings اجرا کنید تا درخواست مرورگر درگیر تغییر مسیر HTTP به HTTPS نشود.

## محدوده آماده‌شده

این قرارداد برای سامانه پیش‌انتخاب و برنامه‌ریزی دستی ترم است. مدیریت کامل اطلاعات پایه، ثبت نمره، کارنامه، حذف ثبت‌نام، موتور زمان‌بندی خودکار و بازیابی رمز جزو UI فعلی نیستند. حساب، چارت، درس و ترم موجود در دیتابیس را استفاده کنید؛ داده تست فقط در دیتابیس موقت ساخته می‌شود و دیتابیس اصلی در این مرحله تغییر نمی‌کند.
