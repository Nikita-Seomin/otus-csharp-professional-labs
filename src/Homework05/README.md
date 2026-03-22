MediaFile — базовый абстрактный класс медиафайла: Id, Title, Metadata (словарь), Tags (список). Тут есть общая “мутируемая” часть, чтобы было видно, что клон должен быть глубоким.
AudioFile — расширяет MediaFile: добавляет Duration и Codec.
MusicTrack — расширяет AudioFile: добавляет Artist и Album.
LiveRecording — расширяет MusicTrack: добавляет Venue и RecordingDate.