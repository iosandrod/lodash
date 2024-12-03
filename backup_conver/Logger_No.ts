import * as log4js from 'log4js';
import * as fs from 'fs';
import * as path from 'path';

namespace Algorithm {
    export class Logger {
        private static _logger: Logger | null = null;
        private _log: log4js.Logger;

        private constructor() {
            const configFile = new ConfigFileCreate().getConfigFile();
            log4js.configure(configFile);
            this._log = log4js.getLogger('ZKERPLog');
        }

        public static get instance(): Logger {
            if (Logger._logger === null) {
                Logger._logger = new Logger();
            }
            return Logger._logger;
        }

        public fatal(message: string, form: string, event: string = ""): void {
            message += "模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.fatal(message);
        }

        public fatalWithException(message: string, exception: Error, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.fatal(message, exception);
        }

        public warn(message: string, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.warn(message);
        }

        public warnWithException(message: string, exception: Error, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.warn(message, exception);
        }

        public error(message: string, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.error(message);
        }

        public errorWithException(message: string, exception: Error, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.error(message, exception);
        }

        public debug(message: string, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.debug(message);
        }

        public info(message: string, form: string, event: string = ""): void {
            message += ";模块:" + form;
            if (event !== "") message += ";事件:" + event;
            this._log.info(message);
        }

        public clearFile(): void {
            const cFile = path.join(process.cwd(), 'Log', 'Logger.log');
            fs.writeFileSync(cFile, '');
        }

        public openFile(cFile: string = ""): void {
            if (cFile === "") {
                cFile = path.join(process.cwd(), 'Log', 'Logger.log');
            }
            require('child_process').exec(`start ${cFile}`);
        }
    }

    class ConfigFileCreate {
        private static readonly Log4Config = "log4net.xml";

        public getConfigFile(): string {
            const assemblyCatalogue = this.getAssemblyCatalogue();
            const fileInfo = path.join(assemblyCatalogue, ConfigFileCreate.Log4Config);
            if (!fs.existsSync(fileInfo)) {
                this.createXMLConfig(fileInfo);
            }
            return fileInfo;
        }

        private getAssemblyCatalogue(): string {
            return process.cwd();
        }

        private createXMLConfig(filePath: string): void {
            const content = `<?xml version="1.0" encoding="utf-8" ?>
<configuration>
 <log4net><!-- contain the Log2net component 's configuration information -->
<root>
<!-- base configuration-->
<level value="Debug"></level>
<appender-ref ref="RootAppender" />
</root>
<appender name="RootAppender" type="log4net.Appender.RollingFileAppender">
<file value="${path.dirname(filePath)}\\Log\\Logger.log" />
<appendToFile value="true" />
 <rollingStyle value="Composite" />
<datePattern value="yyyyMMdd" />
<param name="MaxSizeRollBackups" value="-1" />
<param name="MaximumFileSize" value="10MB" />
<lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
<layout type="log4net.Layout.PatternLayout" >
<conversionPattern value="%date [%c]-[%p] %m%n" />
</layout>
</appender>
<logger name="Test">
<level value="Debug"/>   
<appender-ref ref="RootAppender" />
</logger>
</log4net>
</configuration>`;

            fs.writeFileSync(filePath, content);
        }
    }
}

